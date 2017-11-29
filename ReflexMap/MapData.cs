using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Esri.ArcGISRuntime.Geometry;
using HMConnection;

namespace ReflexMap
{
    public class MapData
    {
        static public List<IGeoInfo> GetDataList(HMCon hmConn, List<DataRow> rowList, IMapLayer layer)
        {
            if (rowList.Count == 0)
                return new List<IGeoInfo>();

            string linkCodeList = GetContentListString(rowList, layer.LinkColumn);
            if(layer is MapPointLayer)
                return GetPointList(hmConn, rowList, layer.ConvertTo<MapPointLayer>(), linkCodeList);
            else if (layer is MapShapeLayer)
                return GetShapeList(hmConn, rowList, layer.ConvertTo<MapShapeLayer>(), linkCodeList);
            else if (layer is MapLineLayer)
                return GetLineList(hmConn, rowList, layer.ConvertTo<MapLineLayer>(), linkCodeList);
            else if (layer is MapEventLayer)
                return GetEventList(hmConn, rowList, layer.ConvertTo<MapEventLayer>(), linkCodeList);

            return null;
        }

        static private List<IGeoInfo> GetPointList(HMCon hmConn, List<DataRow> rowList, MapPointLayer layer, string linkCodeList)
        {
            List<IGeoInfo> pointList = CreateList<PointGeoInfo>(layer, rowList);

            string sql = $"select * from Geo_Link where LinkTableName='{layer.LinkTable}' and Feature='{layer.LayerName}' and LinkCode in ({linkCodeList})";
            var linkTable = hmConn.SQLExecutor.ExecuteDataAdapter(sql, hmConn.TRConnection);

            string pointIdList = "";
            linkTable.Select().ToList().ForEach(row => pointIdList += $",{row["LinkId"]}");
            if (string.IsNullOrEmpty(pointIdList))
            {
                return pointList;
            }

            pointIdList = pointIdList.Remove(0, 1);
            sql = $"select * from Geo_Point where Id in ({pointIdList})";
            var geoTable = hmConn.SQLExecutor.ExecuteDataAdapter(sql, hmConn.TRConnection);

            foreach (var row in rowList)
            {
                PointGeoInfo info = pointList.Find(x => x.KeyCode == $"{row[layer.LinkColumn]}").ConvertTo<PointGeoInfo>();
                var linkRow = linkTable.Select($"LinkCode='{row[layer.LinkColumn]}'");

                if (linkRow.Length > 0)
                {
                    info.PointId = (int)linkRow[0]["LinkId"];
                    var geoRow = geoTable.Select($"Id='{linkRow[0]["LinkId"]}'");
                    if (geoRow.Length > 0)
                    {
                        info.Point = new MapPoint(double.Parse(geoRow[0]["Longitude"].ToString()), double.Parse(geoRow[0]["Latitude"].ToString()), SpatialReferences.Wgs84);
                    }
                }
            }

            return pointList;
        }

        static private List<IGeoInfo> GetShapeList(HMCon hmConn, List<DataRow> rowList, MapShapeLayer layer, string linkCodeList)
        {
            List<IGeoInfo> shapeList = CreateList<ShapeGeoInfo>(layer, rowList);

            string sql = $"select * from Geo_Link where LinkTableName='{layer.LinkTable}' and Feature='{layer.LayerName}' and LinkCode in ({linkCodeList})";
            var linkTable = hmConn.SQLExecutor.ExecuteDataAdapter(sql, hmConn.TRConnection);

            string shapeIdList = "";
            linkTable.Select().ToList().ForEach(row => shapeIdList += $",{row["LinkId"]}");
            if (string.IsNullOrEmpty(shapeIdList))
            {
                return shapeList;
            }

            shapeIdList = shapeIdList.Remove(0, 1);
            sql = $"select * from Geo_Shape where ShapeId in ({shapeIdList})";
            var geoTable = hmConn.SQLExecutor.ExecuteDataAdapter(sql, hmConn.TRConnection);

            foreach(var row in rowList)
            {
                ShapeGeoInfo shape = shapeList.Find(x => x.KeyCode == $"{row[layer.LinkColumn]}").ConvertTo<ShapeGeoInfo>();

                var linkRow = linkTable.Select($"LinkCode='{row[layer.LinkColumn]}'");
                if (linkRow.Length > 0)
                {
                    shape.ShapeId = (int)linkRow[0]["LinkId"];
                    var groups = geoTable.Select($"ShapeId={shape.ShapeId}").GroupBy(x => x["PolygonId"]);
                    foreach (var group in groups)
                    {
                        List<MapPoint> pointList = new List<MapPoint>();
                        group.ToList().ForEach(geo => pointList.Add(new MapPoint(double.Parse(geo["Longitude"].ToString()), double.Parse(geo["Latitude"].ToString()))));
                        Polygon polygon = new Polygon(pointList, SpatialReferences.Wgs84);
                        shape.Polygons.Add(int.Parse($"{group.Key}"), polygon);
                    }
                }
            }

            return shapeList;
        }

        static private List<IGeoInfo> GetLineList(HMCon hmConn, List<DataRow> rowList, MapLineLayer layer, string linkCodeList)
        {
            List<IGeoInfo> lineList = CreateList<LineGeoInfo>(layer, rowList);

            string sql = $"select * from Geo_Link where LinkTableName='{layer.LinkTable}' and Feature='{layer.LayerName}' and LinkCode in ({linkCodeList})";
            var linkTable = hmConn.SQLExecutor.ExecuteDataAdapter(sql, hmConn.TRConnection);

            string polyIdList = "";
            linkTable.Select().ToList().ForEach(row => polyIdList += $",{row["LinkId"]}");
            if (string.IsNullOrEmpty(polyIdList))
            {
                return lineList;
            }

            polyIdList = polyIdList.Remove(0, 1);
            sql = $"select * from Geo_Polyline where PolylineId in ({polyIdList})";
            var geoTable = hmConn.SQLExecutor.ExecuteDataAdapter(sql, hmConn.TRConnection);

            foreach (var row in rowList)
            {
                LineGeoInfo info = lineList.Find(x => x.KeyCode == $"{row[layer.LinkColumn]}").ConvertTo<LineGeoInfo>();

                var linkRow = linkTable.Select($"LinkCode='{row[layer.LinkColumn]}'");
                if (linkRow.Length > 0)
                {
                    info.PolylineId = (int)linkRow[0]["LinkId"];
                    var groups = geoTable.Select($"PolylineId={info.PolylineId}").GroupBy(x => x["LineId"]);
                    foreach (var group in groups)
                    {
                        List<MapPoint> pointList = new List<MapPoint>();
                        group.ToList().ForEach(geo => pointList.Add(new MapPoint(double.Parse(geo["Longitude"].ToString()), double.Parse(geo["Latitude"].ToString()))));
                        Polyline polyline = new Polyline(pointList, SpatialReferences.Wgs84);
                        info.Polylines.Add(int.Parse($"{group.Key}"), polyline);
                    }
                }
            }

            return lineList;
        }

        static private List<IGeoInfo> GetEventList(HMCon hmConn, List<DataRow> rowList, MapEventLayer layer, string linkCodeList)
        {
            List<IGeoInfo> infoList = CreateList<EventGeoInfo>(layer, rowList);

            string sql = $"select * from Geo_Event where EventTypeId ='{layer.TypeId}' and LinkCode in ({linkCodeList})";
            var eventTable = hmConn.SQLExecutor.ExecuteDataAdapter(sql, hmConn.TRConnection);

            foreach (var row in rowList)
            {
                EventGeoInfo info = infoList.Find(x => x.KeyCode == $"{row[layer.LinkColumn]}").ConvertTo<EventGeoInfo>();

                var linkRows = eventTable.Select($"LinkCode='{row[layer.LinkColumn]}'");
                foreach (DataRow data in linkRows)
                {
                    EventDetails eve = new EventDetails();
                    eve.Id = (int)data["Id"];
                    eve.TypeId = layer.TypeId;
                    eve.EventType = layer.EventTypeName;
                    eve.Date = (DateTime)data["EventDate"];
                    eve.Point = new MapPoint(double.Parse(data["Longitude"].ToString()), double.Parse(data["Latitude"].ToString()), SpatialReferences.Wgs84);
                    eve.Description = (string)data["Description"];
                    info.EventList.Add(eve);
                }
            }

            return infoList;
        }

        static public string GetContentListString(List<DataRow> rowList, string field)
        {
            string linkCodeList = "";
            foreach(var row in rowList)
            {
                linkCodeList += $",'{row[field].ToString()}'";
            }
            return linkCodeList.Remove(0, 1);
        }

        static public List<IGeoInfo> CreateList<T>(IMapLayer layer, List<DataRow> rowList) where T : GeoInfo, new()
        {
            List<IGeoInfo> infoList = new List<IGeoInfo>();
            foreach (var row in rowList)
            {
                T info = new T();
                info.KeyCode = $"{row[layer.LinkColumn]}";
                layer.Attributes.ToList().ForEach(attr =>
                {
                    var text = $"{row[attr.LinkField]}";
                    text = attr.LinkDictionary?[text] ?? text;
                    info.AttrList.Add(attr.DisplayName, text);
                });
                infoList.Add(info);
            }

            return infoList;
        }
    }
}
