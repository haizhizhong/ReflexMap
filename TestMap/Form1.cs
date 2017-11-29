using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Esri.ArcGISRuntime.Symbology;
using ReflexMap;

namespace TestMap
{
    public partial class Form1 : Form
    {
        private HMConnection.HMCon hmConn;
        private TUC_HMDevXManager.TUC_HMDevXManager hmDevMgr;

        public Form1()
        {
            InitializeComponent();

            hmConn = new HMConnection.HMCon("web_qa_re", "CSMSQL2012", 12, "ying");
            //hmConn = new HMConnection.HMCon("WEB_DEMO_MASTER_V11", "REFLEXDEMOSERV", 12, "ying");
            hmDevMgr = new TUC_HMDevXManager.TUC_HMDevXManager();

            List<IMapLayer> layers = new List<IMapLayer>();
            MapShapeLayer shapeLayer = new MapShapeLayer()
            {
                LinkTable = "Test",
                LinkColumn = "Asset Code"
            };
            shapeLayer.Attributes.Add( new AttrInfo("Category", "Category"));
            shapeLayer.Attributes.Add(new AttrInfo("Location", "Location"));
            shapeLayer.Attributes.Add(new AttrInfo("Asset Code", "Asset Code"));
            layers.Add(shapeLayer);

            MapPointLayer pointLayer = new MapPointLayer()
            {
                LinkTable = "Test",
                LinkColumn = "Asset Code",
            };
            pointLayer.Attributes.Add(new AttrInfo("Desc", "Description"));
            pointLayer.Attributes.Add(new AttrInfo("Asset Code", "Asset Code"));
            //pointLayer.HilightMarkerSymbol = new PictureMarkerSymbol();
            //((PictureMarkerSymbol)pointLayer.HilightMarkerSymbol).SetSourceAsync(new System.Uri("http://static.arcgis.com/images/Symbols/Basic/GoldShinyPin.png"));
            //pointLayer.MarkerSymbol = new PictureMarkerSymbol();
            //((PictureMarkerSymbol)pointLayer.MarkerSymbol).SetSourceAsync(new System.Uri("http://static.arcgis.com/images/Symbols/Basic/GreenShinyPin.png"));
            layers.Add(pointLayer);

            MapLineLayer lineLayer = new MapLineLayer()
            {
                LinkTable = "Test",
                LinkColumn = "Asset Code"
            };
            lineLayer.Attributes.Add(new AttrInfo("Category", "Category"));
            lineLayer.Attributes.Add(new AttrInfo("Location", "Location"));
            lineLayer.Attributes.Add(new AttrInfo("Asset Code", "Asset Code"));
            layers.Add(lineLayer);

            var eventList = MapEventLayer.GetEventLayers(hmConn, "Test", "Asset Code");
            layers.AddRange(eventList);

            string sql = $"exec sp_FA_SearchBuildingAssets '{hmConn.MLUser}E', 'E'";
            hmConn.SQLExecutor.ExecuteNonQuery(sql, hmConn.TRConnection);

            sql = $"SELECT distinct eqi_code, [Asset Code], Description, Category, Class, Location " +
                $"FROM working_Fixed_Assets_search WHERE(username = '{hmConn.MLUser}E')";

            var table = hmConn.SQLExecutor.ExecuteDataAdapter(sql, hmConn.TRConnection);
            gc.DataSource = table;

            gv.Columns.ToList().ForEach(x => x.Caption = x.Name + "Cap");

            ucMap1.Init(hmConn, hmDevMgr, gv, layers);
        }

        private void Form1_Load(object sender, System.EventArgs e)
        {
            hmDevMgr.AppInit(this.hmConn.MLUser);
            hmDevMgr.FormInit(this);
        }
    }
}
