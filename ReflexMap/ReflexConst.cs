namespace ReflexMap
{
    public static class MapLinkTables
    {
        public static class SupplierMaster 
        {
            public const string LinkTableName = "SupplierMaster";

            public const string PoLayer = "PO Addresses";
            public const string RemitLayer = "Remittance Addresses";

            public const string AttrSupplier = "Supplier";

            public const string AttrPoAddress = "PO Address";
            public const string AttrPoCity = "PO City";
            public const string AttrPoState = "PO State";
            public const string AttrPoPostal = "PO Postal";

            public const string AttrRemitAddress = "Remit Address";
            public const string AttrRemitCity = "Remit City";
            public const string AttrRemitState = "Remit State";
            public const string AttrRemitPostal = "Remit Postal";

            public static MapPointLayer CreatePoLayer(string linkColumn)
            {
                return new MapPointLayer()
                {
                    LinkTable = LinkTableName,
                    LayerName = PoLayer,
                    LinkColumn = linkColumn,
                };
            }

            public static MapPointLayer CreateRemitLayer(string linkColumn)
            {
                return new MapPointLayer()
                {
                    LinkTable = LinkTableName,
                    LayerName = RemitLayer,
                    LinkColumn = linkColumn,
                };
            }
        }

        public static class CustomerMaster                    // point, dbtable:CUSTOMERS  pk:CUSTOMER_CODE
        {
            public const string LinkTableName = "CustomerMaster";

            public const string BillLayer = "Billing Addresses";
            public const string ShipLayer = "Shipping Addresses";

            public const string AttrCustomer = "Customer";

            public const string AttrBillAddress = "Bill Address";
            public const string AttrBillCity = "Bill City";
            public const string AttrBillState = "Bill State";
            public const string AttrBillPostal = "Bill Postal";

            public const string AttrShipAddress = "Ship Address";
            public const string AttrShipCity = "Ship City";
            public const string AttrShipState = "Ship State";
            public const string AttrShipPostal = "Ship Postal";

            public static MapPointLayer CreateBillLayer(string linkColumn)
            {
                return new MapPointLayer()
                {
                    LinkTable = LinkTableName,
                    LayerName = BillLayer,
                    LinkColumn = linkColumn,
                };
            }

            public static MapPointLayer CreateShipLayer(string linkColumn)
            {
                return new MapPointLayer()
                {
                    LinkTable = LinkTableName,
                    LayerName = ShipLayer,
                    LinkColumn = linkColumn,
                };
            }
        }

        public static class LandPurchasers                    // point, dbtable:CUSTOMERS  pk:CUSTOMER_CODE
        {
            public const string LinkTableName = "LandPurchasers";
            public const string BillLayer = "Billing Addresses";

            public const string AttrCustomer = "Customer";
            public const string AttrAddress = "Address";
            public const string AttrCity = "City";
            public const string AttrState = "State";
            public const string AttrPostal = "Postal";

            public static MapPointLayer CreateBillLayer(string linkColumn)
            {
                return new MapPointLayer()
                {
                    LinkTable = LinkTableName,
                    LayerName = BillLayer,
                    LinkColumn = linkColumn,
                };
            }
        }

        public static class EuqipmentMaster                    // point+shape, dbtable:Equip_ID  pk:eqi_code
        {
            public const string LinkTableName = "EquipmentMaster";

            public const string PointLayer = "Point";
            public const string PolygonLayer = "Polygon";

            public const string AttrAssetCode = "Asset Code";
            public const string AttrDescription = "Description";
            public const string AttrCategory = "Category";
            public const string AttrClass = "Class";
            public const string AttrQuantity = "Quantity";

            public static MapPointLayer CreatePointLayer(string linkColumn)
            {
                return new MapPointLayer()
                {
                    LinkTable = LinkTableName,
                    LayerName = PointLayer,
                    LinkColumn = linkColumn,
                };
            }

            public static MapShapeLayer CreatePolygonLayer(string linkColumn)
            {
                return new MapShapeLayer()
                {
                    LinkTable = LinkTableName,
                    LayerName = PolygonLayer,
                    LinkColumn = linkColumn,
                };
            }
        }

        public static class BuildingMaster                    // point+shape, dbtable:Equip_ID  pk:eqi_code
        {
            public const string LinkTableName = "BuildingMaster";

            public const string PointLayer = "Point";
            public const string PolygonLayer = "Polygon";

            public const string AttrAssetCode = "Asset Code";
            public const string AttrDescription = "Description";
            public const string AttrCategory = "Category";
            public const string AttrClass = "Class";

            public static MapPointLayer CreatePointLayer(string linkColumn)
            {
                return new MapPointLayer()
                {
                    LinkTable = LinkTableName,
                    LayerName = PointLayer,
                    LinkColumn = linkColumn,
                };
            }

            public static MapShapeLayer CreatePolygonLayer(string linkColumn)
            {
                return new MapShapeLayer()
                {
                    LinkTable = LinkTableName,
                    LayerName = PolygonLayer,
                    LinkColumn = linkColumn,
                };
            }

        }

        public static class UnitAssetMaster                    // point, dbtable:Equip_ID  pk:eqi_code
        {
            public const string LinkTableName = "UnitAssetMaster";

            public const string PointLayer = "Point";

            public const string AttrAssetCode = "Asset Code";
            public const string AttrDescription = "Description";
            public const string AttrCategory = "Category";
            public const string AttrClass = "Class";

            public static MapPointLayer CreatePointLayer(string linkColumn)
            {
                return new MapPointLayer()
                {
                    LinkTable = LinkTableName,
                    LayerName = PointLayer,
                    LinkColumn = linkColumn,
                };
            }
        }

        public static class EstimateGeneralConstruction                   
        {
            public const string LinkTableName = "EstimateGeneralConstruction";

            public const string PointLayer = "Point";
            public const string PolygonLayer = "Polygon";

            public const string AttrEstimateName = "Estimate Name";
            public const string AttrCustomerProspect = "Customer/Prospect";
            public const string AttrStatus = "Status";

            public const string AttrSiteAddress = "???";
            public const string AttrCity = "???";

            public static MapPointLayer CreatePointLayer(string linkColumn)
            {
                return new MapPointLayer()
                {
                    LinkTable = LinkTableName,
                    LayerName = PointLayer,
                    LinkColumn = linkColumn,
                };
            }

            public static MapShapeLayer CreatePolygonLayer(string linkColumn)
            {
                return new MapShapeLayer()
                {
                    LinkTable = LinkTableName,
                    LayerName = PolygonLayer,
                    LinkColumn = linkColumn,
                };
            }
        }

        public static class EstimateLandDevelopment                    
        {
            public const string LinkTableName = "EstimateLandDevelopment";

            public const string PointLayer = "Point";
            public const string PolygonLayer = "Polygon";

            public const string AttrEstimateName = "Estimate Name";
            public const string AttrCustomerProspect = "Customer/Prospect";
            public const string AttrLandType = "Land Type";
            public const string AttrStatus = "Status";

            public const string AttrSiteAddress = "???";
            public const string AttrCity = "???";

            public static MapPointLayer CreatePointLayer(string linkColumn)
            {
                return new MapPointLayer()
                {
                    LinkTable = LinkTableName,
                    LayerName = PointLayer,
                    LinkColumn = linkColumn,
                };
            }

            public static MapShapeLayer CreatePolygonLayer(string linkColumn)
            {
                return new MapShapeLayer()
                {
                    LinkTable = LinkTableName,
                    LayerName = PolygonLayer,
                    LinkColumn = linkColumn,
                };
            }
        }

        public static class ProjectCostingGeneralConstruction             
        {
            public const string LinkTableName = "ProjectCostingGeneralConstruction";

            public const string PointLayer = "Point";
            public const string PolygonLayer = "Polygon";

            public const string AttrProjectNum = "Project #";
            public const string AttrProjecName = "Project Name";
            public const string AttrStartDate = "Start Date";
            public const string AttrSiteAddress = "Site Address";
            public const string AttrWarehouse = "Warehouse";            //??
            public const string AttrCustomer = "Customer";              
            public const string AttrProjectManager = "Project Manager";
            public const string AttrStatus = "Status";

            public const string AttrExtDesc = "???";
            public const string AttrCity = "???";

            public static MapPointLayer CreatePointLayer(string linkColumn)
            {
                return new MapPointLayer()
                {
                    LinkTable = LinkTableName,
                    LayerName = PointLayer,
                    LinkColumn = linkColumn,
                };
            }

            public static MapShapeLayer CreatePolygonLayer(string linkColumn)
            {
                return new MapShapeLayer()
                {
                    LinkTable = LinkTableName,
                    LayerName = PolygonLayer,
                    LinkColumn = linkColumn,
                };
            }
        }

        public static class ProjectCostingLandDevelopment                    // 
        {
            public const string LinkTableName = "ProjectCostingLandDevelopmen";

            public const string PointLayer = "Point";
            public const string PolygonLayer = "Polygon";

            public const string AttrProjectNum = "Project #";
            public const string AttrProjecName = "Project Name";
            public const string AttrStartDate = "Start Date";
            public const string AttrSiteAddress = "Site Address";
            public const string AttrWarehouse = "Warehouse";                //
            public const string AttrLandType = "Land Type";
            public const string AttrCustomer = "Customer";                  //
            public const string AttrProjectManager = "Project Manager";
            public const string AttrStatus = "Status";

            public const string AttrCity = "???";
            public const string AttrCommunity = "???";

            public static MapPointLayer CreatePointLayer(string linkColumn)
            {
                return new MapPointLayer()
                {
                    LinkTable = LinkTableName,
                    LayerName = PointLayer,
                    LinkColumn = linkColumn,
                };
            }

            public static MapShapeLayer CreatePolygonLayer(string linkColumn)
            {
                return new MapShapeLayer()
                {
                    LinkTable = LinkTableName,
                    LayerName = PolygonLayer,
                    LinkColumn = linkColumn,
                };
            }
        }

        public static class ProjectCostingLotInventory                    
        {
            public const string LinkTableName = "ProjectCostingLotInventory";

            public const string PointLayer = "Point";
            public const string PolygonLayer = "Polygon";

            public const string AttrClass = "Class";
            public const string AttrLot = "Lot";
            public const string AttrBlock = "Block";
            public const string AttrPlan = "Plan";
            public const string AttrAddress = "Address";
            public const string AttrStatus = "Status";
            public const string AttrSubStatus = "Sub Status";
            public const string AttrPurchaser = "Purchaser";

            public static MapPointLayer CreatePointLayer(string linkColumn)
            {
                return new MapPointLayer()
                {
                    LinkTable = LinkTableName,
                    LayerName = PointLayer,
                    LinkColumn = linkColumn,
                };
            }

            public static MapShapeLayer CreatePolygonLayer(string linkColumn)
            {
                return new MapShapeLayer()
                {
                    LinkTable = LinkTableName,
                    LayerName = PolygonLayer,
                    LinkColumn = linkColumn,
                };
            }
        }

        public static class WorkOrderServiceOrdersRequest
        {
            public const string LinkTableName = "ServiceOrdersServiceRequestClipboard";

            public const string PointLayer = "Point";

            public const string AttrServiceType = "Service Type";
            public const string AttrDescription = "Description";
            public const string AttrCustomerName = "Customer Name";
            public const string AttrServiceAddress = "Service Address";
            public const string AttrCity = "City";
            public const string AttrStatus = "Status";

            public static MapPointLayer CreatePointLayer(string linkColumn)
            {
                return new MapPointLayer()
                {
                    LinkTable = LinkTableName,
                    LayerName = PointLayer,
                    LinkColumn = linkColumn,
                };
            }
        }

        public static class WorkOrderServiceOrdersQuote
        {
            public const string LinkTableName = "ServiceOrdersServiceQuote";

            public const string PointLayer = "Point";

            public const string AttrServiceType = "Service Type";
            public const string AttrDescription = "Description";
            public const string AttrCustomerName = "Customer Name";
            public const string AttrServiceAddress = "Service Address";
            public const string AttrCity = "City";
            public const string AttrStatus = "Status";

            public static MapPointLayer CreatePointLayer(string linkColumn)
            {
                return new MapPointLayer()
                {
                    LinkTable = LinkTableName,
                    LayerName = PointLayer,
                    LinkColumn = linkColumn,
                };
            }
        }

        public static class WorkOrderServiceOrdersOrder
        {
            public const string LinkTableName = "ServiceOrdersServiceOrder";

            public const string PointLayer = "Point";

            public const string AttrServiceType = "Service Type";
            public const string AttrDescription = "Description";
            public const string AttrCustomerName = "Customer Name";
            public const string AttrServiceAddress = "Service Address";
            public const string AttrCity = "City";
            public const string AttrStatus = "Status";

            public static MapPointLayer CreatePointLayer(string linkColumn)
            {
                return new MapPointLayer()
                {
                    LinkTable = LinkTableName,
                    LayerName = PointLayer,
                    LinkColumn = linkColumn,
                };
            }
        }

        public static class WorkOrderInstallationOrdersRequest
        {
            public const string LinkTableName = "InstallationOrdersRequestClipboard";

            public const string PointLayer = "Point";

            public const string AttrServiceType = "Service Type";
            public const string AttrDescription = "Description";
            public const string AttrCustomerName = "Customer Name";
            public const string AttrServiceAddress = "Service Address";
            public const string AttrCity = "City";
            public const string AttrStatus = "Status";

            public static MapPointLayer CreatePointLayer(string linkColumn)
            {
                return new MapPointLayer()
                {
                    LinkTable = LinkTableName,
                    LayerName = PointLayer,
                    LinkColumn = linkColumn,
                };
            }
        }

        public static class WorkOrderInstallationOrdersQuote
        {
            public const string LinkTableName = "InstallationOrdersServiceQuote";

            public const string PointLayer = "Point";

            public const string AttrServiceType = "Service Type";
            public const string AttrDescription = "Description";
            public const string AttrCustomerName = "Customer Name";
            public const string AttrServiceAddress = "Service Address";
            public const string AttrCity = "City";
            public const string AttrStatus = "Status";

            public static MapPointLayer CreatePointLayer(string linkColumn)
            {
                return new MapPointLayer()
                {
                    LinkTable = LinkTableName,
                    LayerName = PointLayer,
                    LinkColumn = linkColumn,
                };
            }
        }

        public static class WorkOrderInstallationOrdersOrder
        {
            public const string LinkTableName = "InstallationOrdersServiceOrder";

            public const string PointLayer = "Point";

            public const string AttrServiceType = "Service Type";
            public const string AttrDescription = "Description";
            public const string AttrCustomerName = "Customer Name";
            public const string AttrServiceAddress = "Service Address";
            public const string AttrCity = "City";
            public const string AttrStatus = "Status";

            public static MapPointLayer CreatePointLayer(string linkColumn)
            {
                return new MapPointLayer()
                {
                    LinkTable = LinkTableName,
                    LayerName = PointLayer,
                    LinkColumn = linkColumn,
                };
            }
        }
    }
}
