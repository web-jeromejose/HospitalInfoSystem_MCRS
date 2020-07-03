using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLayer.Data
{
   public class StationDB
    {
       DBHelper dbHelper = new DBHelper("StationDB");

        public List<Station> getStations()
        {
            var stations = new List<Station>();
            try
            {
                stations = dbHelper.ExecuteSQLAndReturnDataTable("SELECT Id,Name,ISNULL(Code,'') Code  FROM Station WHERE DELETED = 0 ORDER BY name").DataTableToList<Station>();
            }
            catch (Exception ex)
            {

                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

            return stations;

        }

        public List<Station> getStationsByOperatorWise()
        {
            var stations = new List<Station>();
            try
            {
                stations = dbHelper.ExecuteSQLAndReturnDataTable("SELECT Id,Name,ISNULL(Code,'') Code  FROM Station WHERE DELETED = 0 and Id IN(4, 95, 103) ORDER BY Id").DataTableToList<Station>();

           
            }
            catch (Exception ex)
            {

                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

            return stations;

        }

        public List<Station> getCanopBillApprover()
        {
            var stations = new List<Station>();
            try
            {
                stations = dbHelper.ExecuteSQLAndReturnDataTable("select employeeid as Id,employeeid +'-' +employeename as Name, '' as Code from canopbillapprover where deleted = '0' order by employeename ").DataTableToList<Station>();


            }
            catch (Exception ex)
            {

                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

            return stations;

        }


        public List<Station> getBillNo(string BillNo)
        {
            var stations = new List<Station>();
            try
            {

                stations = dbHelper.ExecuteSQLAndReturnDataTable("Select canEMPLOYEEID as Id,'' as Code,'' as Name from canopcompanybilldetail where billno = '" + BillNo + "'").DataTableToList<Station>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

            return stations;

        }

        public List<Station> getItemGroup()
        {
            var stations = new List<Station>();
            try
            {
                stations = dbHelper.ExecuteSQLAndReturnDataTable("SELECT Id,Name,'' as  Code from itemgroup where ID IN (7,17) AND deleted=0 order by name").DataTableToList<Station>();
            }
            catch (Exception ex)
            {

                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

            return stations;

        }

    }
}
