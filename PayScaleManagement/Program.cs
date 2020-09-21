using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayScaleManagement
{
    class Program
    {
        static void Display()
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder()
            {
                DataSource = "IN-5CG016FMRT",
                InitialCatalog = "PayScaleDb",
                IntegratedSecurity = true
            };
            using (SqlConnection con = new SqlConnection(builder.ConnectionString))
            {
                const double GRADE_TDS = 0.10;
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("select PayBand,BasicSalary from Pay");
                string cmdText = stringBuilder.ToString();
                Console.WriteLine("PayBand \t Basic \t\t HRA \t\t TA \t\t DA \t\t NetSalary \t InHandSalary");
                con.Open();
                using (SqlCommand cmd = new SqlCommand(cmdText, con))
                {
                    using (SqlDataReader sqlDataReader = cmd.ExecuteReader())
                    {
                        while (sqlDataReader.Read())
                        {
                            Console.Write(sqlDataReader[0] + "\t\t");
                            Console.Write(sqlDataReader[1] + "\t\t");
                            double HRA = (Convert.ToDouble(sqlDataReader["BasicSalary"]) * 0.1);
                            double TA = (Convert.ToDouble(sqlDataReader["BasicSalary"]) * 0.05);
                            double DA = (Convert.ToDouble(sqlDataReader["BasicSalary"]) * 0.05);
                            double netSal = (Convert.ToDouble(sqlDataReader["BasicSalary"]) + HRA + TA + DA);
                            Console.Write(HRA + "\t\t");
                            Console.Write(TA + "\t\t");
                            Console.Write(DA + "\t\t");
                            Console.Write(netSal + "\t\t");
                            double inSal = netSal - (netSal * GRADE_TDS);
                            Console.Write(inSal + "\t");
                            Console.WriteLine("\n");
                        }
                    }
                }
            }
        }
        static void Insert()
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder()
            {
                DataSource = "IN-5CG016FMRT",
                InitialCatalog = "PayScaleDb",
                IntegratedSecurity = true
            };
            using (SqlConnection con = new SqlConnection(builder.ConnectionString))
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("insert into Pay(PayBand,BasicSalary) values(@band,@sal)");
                string cmdText = stringBuilder.ToString();
                Console.WriteLine("Enter PayBand (char like 'A','C'...)");
                char payBand = char.Parse(Console.ReadLine());
                Console.WriteLine("You Have entered "+payBand);
                Console.WriteLine("Enter Basic Salary");
                double basicSalary = double.Parse(Console.ReadLine());
                using (SqlCommand cmd = new SqlCommand(cmdText, con))
                {
                    cmd.Parameters.AddWithValue("@band",char.ToUpper(payBand));
                    cmd.Parameters.AddWithValue("@sal", basicSalary);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    Console.WriteLine("Record Inserted..!!");
                }
            }
        }
        static void Update()
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder()
            {
                DataSource = "IN-5CG016FMRT",
                InitialCatalog = "PayScaleDb",
                IntegratedSecurity = true
            };
            using (SqlConnection con = new SqlConnection(builder.ConnectionString))
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("Update Pay set BasicSalary=@sal where PayBand=@band");
                string cmdText = stringBuilder.ToString();
                Console.WriteLine("Enter PayBand to update (char like 'A','C'...)");
                char payBand = char.Parse(Console.ReadLine());
                Console.WriteLine("Enter Basic Salary to Update");
                double basicSalary = double.Parse(Console.ReadLine());
                using (SqlCommand cmd = new SqlCommand(cmdText, con))
                {
                    cmd.Parameters.AddWithValue("@band", char.ToUpper(payBand));
                    cmd.Parameters.AddWithValue("@sal", basicSalary);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    Console.WriteLine("Record Updated..!!");
                }
            }
        }
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Choose operation");
                Console.WriteLine("1.Insert\n2.Update\n3.Display");
                int op = int.Parse(Console.ReadLine());
                switch (op)
                {
                    case 1:
                        {
                            Insert();
                            break;
                        }
                    case 2:
                        {
                            Update();
                            break;
                        }
                    case 3:
                        {
                            Display();
                            break;
                        }
                    default:
                        {
                            Console.WriteLine("Enter a valid option");
                            break;
                        }
                }
               
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error - " + ex.Message);
            }
            finally
            {
                Console.ReadKey();
            }
        }
    }
}
