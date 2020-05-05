using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YchetTovarov
{
    public class fullList
    {
        public fullList() { }
        public List<StroredProduct> AllstroredProducts = new List<StroredProduct>();
        public List<Product> AllProducts = new List<Product>();
        public List<Postavshick> AllPostavshicks=new List<Postavshick>();
        public List<Zakaz> AllZakazs = new List<Zakaz>();
    }
    public class Zakaz
    {
        public Zakaz() { }
        public Postavshick postavshick;
        public Product stroredProduct;
        public int Count;

    }
    public class StroredProduct
    {
        public StroredProduct() { }
        public Product product;
        public int polka;
        public int count;
        public Postavshick Postavshick;
    }
    public class Product
    {
        public Product() { }
        public string Name;
        public string Desc;

    }
    public class Postavshick
    {
        public Postavshick() { }     
        public string adress, name, tel;
    }


}
