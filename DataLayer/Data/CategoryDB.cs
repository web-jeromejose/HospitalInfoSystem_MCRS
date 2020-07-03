using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataLayer.Model;
namespace DataLayer.Data
{
   public class CategoryDB
    {
        DBHelper dbHelper = new DBHelper("CategoryDB");

        public List<CategoryModel> getCategories()
        {
            var categories = new List<CategoryModel>();
            try
            {
                categories = dbHelper.ExecuteSQLAndReturnDataTable("SELECT Id,Name,Code FROM Category WHERE DELETED = 0 ORDER BY name").DataTableToList<CategoryModel>();
            } 
            catch (Exception ex)
            {

                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

            return categories;

        }
        public List<CategoryModel> getAllCategories()
        {
            var categories = new List<CategoryModel>();
            try
            {
                categories = dbHelper.ExecuteSQLAndReturnDataTable(" select 0 as Id,'ALL' as Name,'0' as Code union all SELECT Id,cast(Code as varchar(max)) + ' - '+cast(Name as varchar(max))  as Name,name as Code  FROM Category WHERE DELETED = 0 ORDER BY Code ").DataTableToList<CategoryModel>();
            }
            catch (Exception ex)
            {

                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

            return categories;

        }

        public CategoryModel getCategory(int categoryId) {

            var category = new CategoryModel();
            try
            {
                category = dbHelper.ExecuteSQLAndReturnDataTable("SELECT Id,Name FROM Category WHERE Id="+ categoryId).DataTableToModel<CategoryModel>();
            }
            catch (Exception ex)
            {

                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

            return category;
        }

        public List<CategoryModel> getSubCategories()
        {

            var category = new List<CategoryModel>();
            try
            {
                category = dbHelper.ExecuteSQLAndReturnDataTable("SELECT Id,Name FROM SUBCATEGORY ORDER BY id").DataTableToList<CategoryModel>();
            }
            catch (Exception ex)
            {

                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

            return category;
        }

        public List<CategoryModel> getCategoriesWithId()
        {
            var categories = new List<CategoryModel>();
            try
            {
                categories = dbHelper.ExecuteSQLAndReturnDataTable("select id,code+'-'+name name from category where id > 1 and deleted = 0 order by name").DataTableToList<CategoryModel>();
            }
            catch (Exception ex)
            {

                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

            return categories;

        }
        public List<CategoryModel> getCategoriesWithOutId()
        {
            var categories = new List<CategoryModel>();
            try
            {
                categories = dbHelper.ExecuteSQLAndReturnDataTable("select id,code+'-'+name name from category where id=1 and deleted = 0 order by name").DataTableToList<CategoryModel>();
            }
            catch (Exception ex)
            {

                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

            return categories;

        }
      
          public List<CategoryModel> getIPBServices()
          {
              var categories = new List<CategoryModel>();
              try
              {
                  categories = dbHelper.ExecuteSQLAndReturnDataTable("select id,servicename name from ipbservice where deleted = 0 order by name").DataTableToList<CategoryModel>();
              }
              catch (Exception ex)
              {

                  throw new ApplicationException(Errors.ExemptionMessage(ex));
              }

              return categories;

          }
        public List<CategoryModel> getOPBServices()
        {
            var categories = new List<CategoryModel>();
            try
            {
                categories = dbHelper.ExecuteSQLAndReturnDataTable("select id,name   from opbservice where deleted = 0 order by name ").DataTableToList<CategoryModel>();
            }
            catch (Exception ex)
            {

                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

            return categories;

        }


    }
}
