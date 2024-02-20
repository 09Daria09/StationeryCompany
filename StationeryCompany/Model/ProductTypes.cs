using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StationeryCompany.Model
{
    public class ProductType
    {
        public int TypeID { get; set; }
        public string TypeName { get; set; }

        public ProductType() { }

        public ProductType(int typeId, string typeName)
        {
            TypeID = typeId;
            TypeName = typeName;
        }
    }

}
