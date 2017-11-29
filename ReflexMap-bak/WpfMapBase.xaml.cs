using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using Esri.ArcGISRuntime.Layers;
using WS_Popups;

namespace ReflexMap
{
    /// <summary>
    /// Interaction logic for WpfMapDrawPoint.xaml
    /// </summary>
    public partial class WpfMapBase : UserControl
    {
        protected TUC_HMDevXManager.TUC_HMDevXManager _hmDevMgr;
        protected frmPopup _pop;

        private Dictionary<string, string> _baseMaps = new Dictionary<string, string>();

        public WpfMapBase(TUC_HMDevXManager.TUC_HMDevXManager hmDevMgr)
        {
            InitializeComponent();
            _hmDevMgr = hmDevMgr;
            _pop = new frmPopup(_hmDevMgr);

            _baseMaps.Add("Open Street", "OpenStreetMap");
            _baseMaps.Add("Streets", "http://services.arcgisonline.com/ArcGIS/rest/services/World_Street_Map/MapServer");
            _baseMaps.Add("Topo", "http://services.arcgisonline.com/ArcGIS/rest/services/World_Topo_Map/MapServer");
            _baseMaps.Add("Imagery", "http://services.arcgisonline.com/ArcGIS/rest/services/World_Imagery/MapServer");
            _baseMaps.Add("National Geographic", "http://services.arcgisonline.com/ArcGIS/rest/services/NatGeo_World_Map/MapServer");

            basemapChooser.ItemsSource = _baseMaps;
            basemapChooser.DisplayMemberPath = "Key";
            basemapChooser.SelectedIndex = 0;
        }

        private void OnBasemapChooserSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (mapView.Map.Layers.Count > 0)
            {
                mapView.Map.Layers.RemoveAt(0);
            }

            KeyValuePair<string, string> selected = (KeyValuePair<string, string>)e.AddedItems[0];
            if (selected.Value == "OpenStreetMap")
            {
                mapView.Map.Layers.Insert(0, new OpenStreetMapLayer());
            }
            else
            {
                mapView.Map.Layers.Insert(0, new ArcGISTiledMapServiceLayer()
                {
                    ServiceUri = selected.Value
                });
            }
        }
    }
}
