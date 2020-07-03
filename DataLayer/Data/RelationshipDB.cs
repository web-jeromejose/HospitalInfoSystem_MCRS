using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLayer.Data
{
    public class RelationshipDB
    {

        DBHelper dbHelper = new DBHelper("RelationshipDB");

        public List<Relationship> getRelationship()
        {
            var relationship = new List<Relationship>();
            try
            {
                relationship = dbHelper.ExecuteSQLAndReturnDataTable("SELECT Id,Name FROM Relationships WHERE DELETED = 0 ORDER BY name").DataTableToList<Relationship>();
            }
            catch (Exception ex)
            {

                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

            return relationship;

        }
    }
}
