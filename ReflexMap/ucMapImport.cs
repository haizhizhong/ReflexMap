using System;
using System.Linq;
using System.Data;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Grid;
using HMConnection;
using System.IO;
using Newtonsoft.Json;
using GeoJSON.Net.Feature;
using DevExpress.XtraEditors.Repository;
using System.Collections.Generic;
using GeoJSON.Net.Geometry;
using WS_Popups;

namespace ReflexMap
{
    internal partial class ucMapImport : DevExpress.XtraEditors.XtraUserControl
    {
        HMCon _hmCon;
        TUC_HMDevXManager.TUC_HMDevXManager _hmDevMgr;
        IMapLayer _layer;
        DataTable _table;
        RepositoryItemLookUpEdit _luAllAction;
        RepositoryItemLookUpEdit _luAction;
        frmPopup _pop;

        static class ImportAction
        {
            public const string Ignore = "--";
            public const string Add = "Add";
            public const string Overwrite = "Overwrite";
            public const string Delete = "Delete";
            public const string Refreshed = "Refreshed";
        };

        public const string strYes = "Yes";
        public const string MatchAttr = "Name";

        static class ColumnNames
        {
            public const string Key = "#";
            public const string OldData = "Old Data";
            public const string NewData = "New Data";
            public const string Action = "Action";

            public const string Description = "Description";
            public const string EventDate = "EventDate";
            public const string TimeStamp = "TimeStamp";
        }


        public ucMapImport()
        {
            InitializeComponent();
        }

        public void Init(HMCon con, TUC_HMDevXManager.TUC_HMDevXManager hmDevMgr, List<DataRow> rowList, IMapLayer layer)
        {
            _hmCon = con;
            _hmDevMgr = hmDevMgr;
            _layer = layer;
            _pop = new frmPopup(_hmDevMgr);

            labelTableName.Text = _layer.LinkTable;
            labelFieldName.Text = layer.LinkColumn;
            labelFeatureName.Text = layer.LayerName;

            _table = new DataTable();
            _table.Columns.Add(new DataColumn(ColumnNames.Key));
            _table.PrimaryKey = new DataColumn[] { _table.Columns[ColumnNames.Key] };
            layer.Attributes.ToList().ForEach(attr => _table.Columns.Add(new DataColumn(attr.DisplayName)));
            _table.Columns.Add(new DataColumn(ColumnNames.OldData));
            _table.Columns.Add(new DataColumn(ColumnNames.NewData));
            _table.Columns.Add(new DataColumn(ColumnNames.Action));

            foreach (var srcRow in rowList)
            {
                var row = _table.NewRow();

                row[ColumnNames.Key] = srcRow[layer.LinkColumn];
                layer.Attributes.ToList().ForEach(attr => row[attr.DisplayName] = attr.LinkDictionary?[$"{srcRow[attr.LinkField]}"] ?? srcRow[attr.LinkField]);
                _table.Rows.Add(row);
            }

            try
            {
                if (rowList.Count > 0)
                {
                    string linkCodeList = MapData.GetContentListString(rowList, layer.LinkColumn);
                    string sql = "";
                    if (layer is MapEventLayer)
                    {
                        sql = $"select * from Geo_Event where EventTypeId={layer.ConvertTo<MapEventLayer>().TypeId} and LinkCode in ({linkCodeList})";
                    }
                    else
                    {
                        sql = $"select * from Geo_Link where LinkTableName='{layer.LinkTable}' and Feature='{layer.LayerName}' and LinkCode in ({linkCodeList})";
                    }
                    var linkTable = _hmCon.SQLExecutor.ExecuteDataAdapter(sql, _hmCon.TRConnection);
                    _table.Select().ToList().ForEach(row => row[ColumnNames.OldData] = linkTable.Select($"LinkCode='{row[ColumnNames.Key]}'").Length > 0 ? strYes : string.Empty);
                    _table.Select().ToList().ForEach(row => row[ColumnNames.Action] = ImportAction.Ignore);
                }
            }
            catch (Exception ex)
            {
                _pop.ShowPopup($"Error in loading: {ex.Message}");
            }

            gc.DataSource = _table;

            _luAllAction = new RepositoryItemLookUpEdit();
            _luAllAction.DataSource = new string[] { ImportAction.Ignore, ImportAction.Add, ImportAction.Overwrite, ImportAction.Delete };
            gc.RepositoryItems.Add(_luAllAction);
            gv.Columns["Action"].ColumnEdit = _luAllAction;

            gv.Columns.ToList().ForEach(col => col.OptionsColumn.AllowEdit = col.FieldName == ColumnNames.Action);

            _luAction = new RepositoryItemLookUpEdit();
        }

        private void btnFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog frm = new OpenFileDialog())
            {
                frm.DefaultExt = "geojson";
                frm.Filter = "geojson files (*.geojson)|*.geojson";
                frm.InitialDirectory = " C:\\";
                frm.RestoreDirectory = true;
                frm.Title = "Import data from a GeoJson file";
                frm.CheckFileExists = true;

                if (frm.ShowDialog() == DialogResult.OK)
                {
                    txtFile.Text = frm.FileName;

                    try
                    {
                        var content = File.ReadAllText(txtFile.Text);
                        FeatureCollection fs = JsonConvert.DeserializeObject<FeatureCollection>(content);

                        _table.Select().ToList().ForEach(row => { row[ColumnNames.NewData] = string.Empty; row[ColumnNames.Action] = ImportAction.Ignore; });
                        foreach (Feature f in fs.Features)
                        {
                            string code = f.Properties[MatchAttr].ToString();
                            var row = _table.Rows.Find($"{code}");
                            if (row != null)
                            {
                                if (IsValidType(f.Geometry))
                                {
                                    row[ColumnNames.NewData] = strYes;
                                    row[ColumnNames.Action] = ($"{row[ColumnNames.OldData]}" == strYes) ? ImportAction.Overwrite : ImportAction.Add;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _pop.ShowPopup("Cannot read the file, please ensure that you've provided a valid GeoJson import file format. " + ex.Message);
                    }
                }
            }
        }

        private bool IsValidType(IGeometryObject g)
        {
            if (_layer is MapPointLayer)
            {
                return g is Point;
            }
            if (_layer is MapShapeLayer)
            {
                return (g is Polygon) || (g is MultiPolygon);
            }
            if (_layer is MapLineLayer)
            {
                return (g is LineString) || (g is MultiLineString);
            }
            if (_layer is MapEventLayer)
            {
                return g is Point;
            }

            return false;
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            try
            {
                DeleteOldData();

                if (_table.Select($"{ColumnNames.Action}<>'{ImportAction.Ignore}'").Count() > 0)
                {
                    ImportFile();
                }

                _pop.ShowPopup("Processing Has Completed.");
            }
            catch (Exception ex)
            {
                _pop.ShowPopup("Cannot execute. " + ex.Message);
            }
        }

        private void DeleteOldData()
        {
            for (int i = 0; i < gv.RowCount; i++)
            {
                var row = gv.GetDataRow(i);
                if (new string[] { ImportAction.Delete, ImportAction.Overwrite }.Contains($"{row["Action"]}"))
                {
                    if (_layer is MapEventLayer)
                    {
                        _hmCon.SQLExecutor.ExecuteNonQuery($"delete Geo_Event where LinkCode='{row[ColumnNames.Key]}' and EventTypeId={_layer.ConvertTo<MapEventLayer>().TypeId}", _hmCon.TRConnection);
                    }
                    else
                    {
                        string sql = $"select * from Geo_Link where LinkTableName='{_layer.LinkTable}' and Feature='{_layer.LayerName}' and LinkCode='{row[ColumnNames.Key]}'";
                        DataTable table = _hmCon.SQLExecutor.ExecuteDataAdapter(sql, _hmCon.TRConnection);
                        int oldLinkId = (int)table.Rows[0]["LinkId"];
                        int oldId = (int)table.Rows[0]["Id"];

                        _hmCon.SQLExecutor.ExecuteNonQuery($"delete Geo_Link where Id={oldId}", _hmCon.TRConnection);

                        if (_layer is MapShapeLayer)
                        {
                            _hmCon.SQLExecutor.ExecuteNonQuery($"delete Geo_Shape where ShapeId={oldLinkId}", _hmCon.TRConnection);
                        }
                        else if (_layer is MapLineLayer)
                        {
                            _hmCon.SQLExecutor.ExecuteNonQuery($"delete Geo_Polyline where PolylineId={oldLinkId}", _hmCon.TRConnection);
                        }
                        else if (_layer is MapPointLayer)
                        {
                            _hmCon.SQLExecutor.ExecuteNonQuery($"delete Geo_Point where Id={oldLinkId}", _hmCon.TRConnection);
                        }
                    }

                    row[ColumnNames.OldData] = string.Empty;
                    if ($"{row[ColumnNames.Action]}" == ImportAction.Delete)
                    {
                        row[ColumnNames.Action] = ImportAction.Ignore;
                    }
                }
            }
        }

        private void ImportFile()
        {
            object temp = _hmCon.SQLExecutor.ExecuteScalar("select max(ShapeId) from Geo_Shape", _hmCon.TRConnection);
            int shapeId = (DBNull.Value.Equals(temp)) ? 0 : (int)temp;

            temp = _hmCon.SQLExecutor.ExecuteScalar("select max(PolylineId) from Geo_Polyline", _hmCon.TRConnection);
            int polylineId = (DBNull.Value.Equals(temp)) ? 0 : (int)temp;

            var content = File.ReadAllText(txtFile.Text);
            FeatureCollection fs = JsonConvert.DeserializeObject<FeatureCollection>(content);

            foreach (Feature f in fs.Features)
            {
                string code = $"{f.Properties[MatchAttr]}";
                var row = _table.Rows.Find($"{code}");
                if (row == null || $"{row[ColumnNames.Action]}" == ImportAction.Ignore)
                    continue;

                if (new string[] { ImportAction.Add, ImportAction.Overwrite, ImportAction.Refreshed }.Contains($"{row[ColumnNames.Action]}"))
                {
                    if (_layer is MapShapeLayer)
                    {
                        ImportOneShape(f, ref shapeId, code);
                    }
                    else if (_layer is MapLineLayer)
                    {
                        ImportOneLine(f, ref polylineId, code);
                    }
                    else if (_layer is MapPointLayer)
                    {
                        ImportOnePoint(f, code);
                    }
                    else if (_layer is MapEventLayer)
                    {
                        ImportOneEvent(f, code);
                    }

                    row[ColumnNames.OldData] = strYes;
                    row[ColumnNames.NewData] = string.Empty;
                    row[ColumnNames.Action] = ImportAction.Refreshed;
                }
            }

            for (int i = 0; i < gv.RowCount; i++)
            {
                var row = gv.GetDataRow(i);
                row[ColumnNames.Action] = (row[ColumnNames.Action].ToString() == ImportAction.Refreshed) ? ImportAction.Ignore : row[ColumnNames.Action];
            }
        }

        void ImportOneShape(Feature f, ref int shapeId, string code)
        {
            List<Polygon> polyList = new List<Polygon>();
            if (f.Geometry is MultiPolygon)
            {
                polyList = (f.Geometry as MultiPolygon).Coordinates;
            }
            else if (f.Geometry is Polygon)
            {
                polyList.Add(f.Geometry as Polygon);
            }

            shapeId++;
            int polygonId = 1;
            foreach (var polygon in polyList)
            {
                foreach (var p in polygon.Coordinates[0].Coordinates)
                {
                    string sql = $"insert into Geo_Shape(ShapeId, Latitude, Longitude, PolygonId) values({shapeId}, {p.Latitude}, {p.Longitude}, {polygonId})";
                    _hmCon.SQLExecutor.ExecuteNonQuery(sql, _hmCon.TRConnection);
                }
                polygonId++;
            }

            string sqlInsertLink = $"insert into Geo_Link(LinkTableName, Feature, LinkCode, LinkType, LinkId)" +
                $" values('{_layer.LinkTable}','{_layer.LayerName}', '{code}', {(int)GeoType.Shape}, {shapeId})";
            _hmCon.SQLExecutor.ExecuteNonQuery(sqlInsertLink, _hmCon.TRConnection);
        }

        void ImportOneLine(Feature f, ref int polylineId, string code)
        {
            List<LineString> polyList = new List<LineString>();
            if (f.Geometry is MultiLineString)
            {
                polyList = (f.Geometry as MultiLineString).Coordinates;
            }
            else if (f.Geometry is LineString)
            {
                polyList = new List<LineString>();
                polyList.Add(f.Geometry as LineString);
            }

            polylineId++;
            int lineId = 1;
            foreach (var poly in polyList)
            {
                foreach (var p in poly.Coordinates)
                {
                    string sql = $"insert into Geo_Polyline(PolylineId, Latitude, Longitude, LineId) values({polylineId}, {p.Latitude}, {p.Longitude}, {lineId})";
                    _hmCon.SQLExecutor.ExecuteNonQuery(sql, _hmCon.TRConnection);
                }
                lineId++;
            }

            string sqlInsertLink = $"insert into Geo_Link(LinkTableName, Feature, LinkCode, LinkType, LinkId)" +
                $" values('{_layer.LinkTable}','{_layer.LayerName}', '{code}', {(int)GeoType.Polyline}, {polylineId})";
            _hmCon.SQLExecutor.ExecuteNonQuery(sqlInsertLink, _hmCon.TRConnection);
        }

        void ImportOnePoint(Feature f, string code)
        {
            Point point = f.Geometry as Point;

            string sql = $"insert into Geo_Point( Latitude, Longitude, TimeStamp) " +
            $"values({point.Coordinates.Latitude}, {point.Coordinates.Longitude}, '{f.Properties[ColumnNames.TimeStamp]}'); SELECT SCOPE_IDENTITY()";
            int pointId = Convert.ToInt32(_hmCon.SQLExecutor.ExecuteScalar(sql, _hmCon.TRConnection));

            sql = $"insert into Geo_Link(LinkTableName, LinkCode, Feature, LinkType, LinkId)" +
                $" values('{_layer.LinkTable}','{code}', '{_layer.LayerName}', {(int)GeoType.Point}, {pointId})";
            _hmCon.SQLExecutor.ExecuteNonQuery(sql, _hmCon.TRConnection);
        }

        void ImportOneEvent(Feature f, string code)
        {
            Point point = f.Geometry as Point;

            string sql = $"insert into Geo_Event(LinkCode, Latitude, Longitude, EventTypeId, Description, EventDate)" +
                $" values('{code}', {point.Coordinates.Latitude}, {point.Coordinates.Longitude}, {_layer.ConvertTo<MapEventLayer>().TypeId}, " +
                $"'{f.Properties[ColumnNames.Description]}', '{f.Properties[ColumnNames.EventDate]}')";
            _hmCon.SQLExecutor.ExecuteNonQuery(sql, _hmCon.TRConnection);
        }

        private void gv_CustomRowCellEditForEditing(object sender, CustomRowCellEditEventArgs e)
        {
            if (e.Column.FieldName == ColumnNames.Action)
            {
                DataRow dr = gv.GetDataRow(e.RowHandle);
                if (dr != null)
                {
                    if ($"{dr[ColumnNames.NewData]}" == strYes)
                    {
                        if ($"{dr[ColumnNames.OldData]}" == strYes)
                            _luAction.DataSource = new string[] { ImportAction.Ignore, ImportAction.Overwrite, ImportAction.Delete };
                        else
                            _luAction.DataSource = new string[] { ImportAction.Ignore, ImportAction.Add };
                    }
                    else
                    {
                        if ($"{dr[ColumnNames.OldData]}" == strYes)
                            _luAction.DataSource = new string[] { ImportAction.Ignore, ImportAction.Delete };
                        else
                            _luAction.DataSource = new string[] { ImportAction.Ignore };
                    }

                    e.RepositoryItem = _luAction;
                }
            }
        }

        private void btnIgnoreAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < gv.RowCount; i++)
            {
                var row = gv.GetDataRow(i);
                row[ColumnNames.Action] = ImportAction.Ignore;
            }
        }

        private void btnImportAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < gv.RowCount; i++)
            {
                var row = gv.GetDataRow(i);
                if ($"{row[ColumnNames.NewData]}" == strYes)
                {
                    if ($"{row[ColumnNames.OldData]}" == strYes)
                        row[ColumnNames.Action] = ImportAction.Overwrite;
                    else
                        row[ColumnNames.Action] = ImportAction.Add;
                }
            }
        }

        private void btnDeleteAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < gv.RowCount; i++)
            {
                var row = gv.GetDataRow(i);
                if ($"{row[ColumnNames.OldData]}" == strYes)
                {
                    row[ColumnNames.Action] = ImportAction.Delete;
                }
            }
        }

        private void ucMapImport_Load(object sender, EventArgs e)
        {
            _hmDevMgr.FormInit(this);
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog frm = new OpenFileDialog())
            {
                frm.DefaultExt = "geojson";
                frm.Filter = "geojson files (*.geojson)|*.geojson";
                frm.InitialDirectory = " C:\\";
                frm.RestoreDirectory = true;
                frm.Title = "Export GeoJson file as";
                frm.CheckFileExists = false;

                if (frm.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        txtFile.Text = frm.FileName;

                        List<Feature> list = new List<Feature>();
                        for (int i = 0; i < gv.RowCount; i++)
                        {
                            var row = gv.GetDataRow(i);
                            if ($"{row[ColumnNames.OldData]}" == strYes)
                            {
                                if (_layer is MapEventLayer)
                                {
                                    ExportOneRowEvents(row, list);
                                }
                                else
                                {
                                    string sql = $"select * from Geo_Link where LinkTableName='{_layer.LinkTable}' and Feature='{_layer.LayerName}' and LinkCode='{row[ColumnNames.Key]}'";
                                    DataTable linkTable = _hmCon.SQLExecutor.ExecuteDataAdapter(sql, _hmCon.TRConnection);
                                    int oldLinkId = (int)linkTable.Rows[0]["LinkId"];

                                    if (_layer is MapPointLayer)
                                    {
                                        ExportOnePoint(row, oldLinkId, list);
                                    }
                                    else if (_layer is MapShapeLayer)
                                    {
                                        ExportOneShape(row, oldLinkId, list);
                                    }
                                    else if (_layer is MapLineLayer)
                                    {
                                        ExportOnePolyline(row, oldLinkId, list);
                                    }
                                }
                            }
                        }

                        var json = JsonConvert.SerializeObject(new FeatureCollection(list));
                        System.IO.File.WriteAllText(txtFile.Text, json);

                        _pop.ShowPopup("The Export Has Completed.");
                    }
                    catch (Exception ex)
                    {
                        _pop.ShowPopup("Cannot execute. " + ex.Message);
                    }
                }
            }
        }

        void ExportOneRowEvents(DataRow row, List<Feature> list)
        {
            string sql = $"select * from Geo_Event where EventTypeId={_layer.ConvertTo<MapEventLayer>().TypeId} and LinkCode='{row[ColumnNames.Key]}'";
            DataTable geoTable = _hmCon.SQLExecutor.ExecuteDataAdapter(sql, _hmCon.TRConnection);
            foreach (DataRow geo in geoTable.Rows)
            {
                double longitude = double.Parse(geo["Longitude"].ToString());
                double latitude = double.Parse(geo["Latitude"].ToString());

                var attr = AttrInfo.GenAttr(MatchAttr, row[ColumnNames.Key]);
                attr.Add(ColumnNames.Description, geo[ColumnNames.Description].ToString());
                attr.Add(ColumnNames.EventDate, geo[ColumnNames.EventDate].ToString());
                list.Add(new Feature(new Point(new Position(latitude, longitude)), attr));
            }
        }

        void ExportOnePoint(DataRow row, int oldLinkId, List<Feature> list)
        {
            string sql = $"select * from Geo_Point where id={oldLinkId}";
            DataTable geoTable = _hmCon.SQLExecutor.ExecuteDataAdapter(sql, _hmCon.TRConnection);
            double longitude = double.Parse(geoTable.Rows[0]["Longitude"].ToString());
            double latitude = double.Parse(geoTable.Rows[0]["Latitude"].ToString());

            var attr = AttrInfo.GenAttr(MatchAttr, row[ColumnNames.Key]);
            attr.Add(ColumnNames.TimeStamp, geoTable.Rows[0][ColumnNames.TimeStamp].ToString());

            list.Add(new Feature(new Point(new Position(latitude, longitude)), AttrInfo.GenAttr(MatchAttr, row[ColumnNames.Key])));
        }

        void ExportOneShape(DataRow row, int oldLinkId, List<Feature> list)
        {
            string sql = $"select * from Geo_Shape where ShapeId={oldLinkId}";
            DataTable geoTable = _hmCon.SQLExecutor.ExecuteDataAdapter(sql, _hmCon.TRConnection);
            var groups = geoTable.Select().GroupBy(x => x["PolygonId"]);
            List<Polygon> polygonList = new List<Polygon>();
            foreach (var group in groups)
            {
                List<Position> pointList = new List<Position>();
                group.ToList().ForEach(geo => pointList.Add(new Position(double.Parse(geo["Latitude"].ToString()), double.Parse(geo["Longitude"].ToString()))));
                polygonList.Add(new Polygon(new List<LineString> { new LineString(pointList) }));
            }

            list.Add(new Feature(polygonList.Count == 1 ? polygonList[0] as IGeometryObject : new MultiPolygon(polygonList), AttrInfo.GenAttr(MatchAttr, row[ColumnNames.Key])));
        }

        void ExportOnePolyline(DataRow row, int oldLinkId, List<Feature> list)
        {
            string sql = $"select * from Geo_Polyline where PolylineId={oldLinkId}";
            DataTable geoTable = _hmCon.SQLExecutor.ExecuteDataAdapter(sql, _hmCon.TRConnection);
            var groups = geoTable.Select().GroupBy(x => x["LineId"]);
            List<LineString> lineList = new List<LineString>();
            foreach (var group in groups)
            {
                List<Position> pointList = new List<Position>();
                group.ToList().ForEach(geo => pointList.Add(new Position(double.Parse(geo["Latitude"].ToString()), double.Parse(geo["Longitude"].ToString()))));
                lineList.Add(new LineString(pointList));
            }

            list.Add(new Feature(lineList.Count == 1 ? lineList[0] as IGeometryObject : new MultiLineString(lineList), AttrInfo.GenAttr(MatchAttr, row[ColumnNames.Key])));
        }
    }
}
