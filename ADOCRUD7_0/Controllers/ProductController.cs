using ADOCRUD7_0.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace ADOCRUD7_0.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ProductController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        [Route("GetAllProduct")]
        [HttpGet]
        public async Task<IActionResult> GetAllProduct()
        {
            List<ProductModel> productModels = new List<ProductModel>();
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            SqlCommand cmd = new SqlCommand("SELECT * FROM TBLProduct", con);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(dt);
            for(int i=0;i<dt.Rows.Count;i++)
            {
                ProductModel productModel = new ProductModel();
                productModel.ProductID = Convert.ToInt32(dt.Rows[i]["ID"]);
                productModel.ProductName = dt.Rows[i]["Pname"].ToString();
                productModel.ProductPrice = Convert.ToDecimal(dt.Rows[i]["PPrice"]);
                productModel.EntryDate = Convert.ToDateTime(dt.Rows[i]["PEntryDate"]);
                productModels.Add(productModel);
            }
            return Ok(productModels);
        }


        [Route("PostProduct")]
        [HttpPost]
        public async Task<IActionResult> PostProduct(ProductModel obj)
        {
            try
            {
                SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
                SqlCommand cmd = new SqlCommand("Insert into TBLProduct values ('" + obj.ProductName + "','" + obj.ProductPrice + "',getdate())", con);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                return Ok(obj);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        [Route("UpdateProduct")]
        [HttpPost]
        public async Task<IActionResult> UpdateProduct(ProductModel obj)
        {
            try
            {
                SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
                SqlCommand cmd = new SqlCommand("Update TBLProduct set Pname='" + obj.ProductName + "',Pprice='" + obj.ProductPrice + "' where id='"+obj.ProductID+"'", con);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                return Ok(obj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
