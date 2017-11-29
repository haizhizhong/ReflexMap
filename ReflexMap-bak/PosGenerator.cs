using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Xml.Linq;
using GeoJSON.Net.Geometry;
using HMConnection;

namespace ReflexMap
{
    static class PosGenColumnNames
    {
        public const string Key = "#";
        public const string Status = "Status";
        public const string Address = "Address";
        public const string Exception = "Exception";
    }

    static class PosGenStatus
    {
        public const string Generated = "Generated";
        public const string Failed = "Failed";
        public const string Exception = "Exception";
    }

    public class PosGenerator
    {
        public List<string> ColumnAddress = new List<string>();
        public string ColumnCity;
        public string ColumnState;
        public string ColumnCountry;
        public string ColumnPostalCode;

        public string LinkTable;
        public string LayerName;
        public string LinkColumn;

        public PosGenerator(MapPointLayer layer)
        {
            LinkTable = layer.LinkTable;
            LayerName = layer.LayerName;
            LinkColumn = layer.LinkColumn;
        }

        public string GetPart(DataRow row, string part)
        {
            return string.IsNullOrEmpty(part) ? "" : $", {row[part]}";
        }

        public string GetAddress(DataRow row)
        {
            string address = "";
            foreach (var col in ColumnAddress)
            {
                address += string.IsNullOrWhiteSpace($"{row[col]}") ? "" : $", {row[col]}";
            }

            return address.Length > 2 ? address.Remove(0, 2) : address;
        }

        public virtual List<DataRow> GetData(List<DataRow> src)
        {
            return src;
        }

        public bool RecordExist(HMCon hmCon, string keyCode)
        {
            string sql = $"select count(*) from Geo_Link where LinkTableName='{LinkTable}' and Feature='{LayerName}' and LinkCode='{keyCode}'";
            int count = Convert.ToInt32(hmCon.SQLExecutor.ExecuteScalar(sql, hmCon.TRConnection));

            return count>0;
        }

        public void Save(HMCon hmCon, string keyCode, double lat, double lon)
        {
            string sql = $"insert into Geo_Point( Latitude, Longitude, TimeStamp) " +
            $"values({lat}, {lon}, '{DateTime.Now.ToShortDateString()}'); SELECT SCOPE_IDENTITY()";
            int pointId = Convert.ToInt32(hmCon.SQLExecutor.ExecuteScalar(sql, hmCon.TRConnection));

            sql = $"insert into Geo_Link(LinkTableName, LinkCode, Feature, LinkType, LinkId)" +
                $" values('{LinkTable}','{keyCode}', '{LayerName}', {(int)GeoType.Point}, {pointId})";
            hmCon.SQLExecutor.ExecuteNonQuery(sql, hmCon.TRConnection);
        }

        public string GetFullAddress(DataRow row)
        {
            return GetAddress(row) + GetPart(row, ColumnCity) + GetPart(row, ColumnState) + GetPart(row, ColumnPostalCode) + GetPart(row, ColumnCountry);
        }

        public static Position GetPositionFromAddress(string fullAddress)
        {
            var requestUri = string.Format("http://maps.googleapis.com/maps/api/geocode/xml?address={0}&sensor=false", Uri.EscapeDataString(fullAddress));

            var request = WebRequest.Create(requestUri);
            var response = request.GetResponse();
            var xdoc = XDocument.Load(response.GetResponseStream());

            var result = xdoc.Element("GeocodeResponse")?.Element("result");
            if (result != null)
            {
                var locationElement = result.Element("geometry").Element("location");

                var lat = double.Parse(locationElement.Element("lat").Value);
                var lng = double.Parse(locationElement.Element("lng").Value);

                return new Position(lat, lng);
            }
            else
            {
                return null;
            }
        }
    }
}
