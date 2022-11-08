using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Runtime.Serialization;
using System.Globalization;

namespace FYP.Project
{
    public partial class payroll : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
                   
        }

        private double pcbRoundUp(double value)
        {
            double roundUpValue = value;

            double dblTemp = roundUpValue; // 6.375
            int intTemp = (int)roundUpValue; // 6
            int intTemp2 = 0;
            double dblTemp2 = intTemp; // 6
            dblTemp = dblTemp - intTemp; //6.375 - 6 = 0.375
            dblTemp = (int)(dblTemp * 100); //0.375 = 37
            intTemp2 = (int)dblTemp; //37
            dblTemp = (int)(dblTemp % 10); //37 = 7
            if (dblTemp == 0 || dblTemp == 5)
            {
                //do nothing
            }
            else if (dblTemp > 5)
            {
                int x = 10 - (int)dblTemp; //10 - 7 = 3
                intTemp2 = intTemp2 + x; //37 + 3 = 40 
            }
            else
            {
                int x = 5 - (int)dblTemp; //5 - 1 = 4
                intTemp2 = intTemp2 + x; // 41 + 4 = 45
            }
            dblTemp = (double)intTemp2 / 100; //40 / 100 = 0.40  
            dblTemp2 += dblTemp; // 6 + 0.40 = 6.40
            roundUpValue = dblTemp2;

            return roundUpValue;
        }

        private void EpfCalculation()
        {
            //epf calculation
            bool exit = false;
            double epfEmployer = 0.0, epfEmployee = 0.0;
            double salary = Convert.ToDouble(TextBox1.Text);
            double increaseValue = 20.00;
            double bottomValue = 20.01, topValue = 40.00;
            double range = 0;
            double epfEmployeePercentage = 0.11, epfEmployerPercentage = 0.13;
            int age = 0;

            if (salary >= 0.01 && salary <= 10.00)
            {
                epfEmployer = 0.00;
                epfEmployee = 0.00;
            }
            else if (salary >= 10.01 && salary <= 20.00)
            {
                epfEmployer = 3.00;
                epfEmployee = 3.00;
            }
            else
            {
                //this is default percentage follow the kwsp website
                //if later i let the user choose their percentage, then need to add a boolean value like "true" then skip this step
                if (salary <= 5000 && age >= 60)
                {
                    epfEmployeePercentage = 0.055;
                    epfEmployerPercentage = 0.065;
                }
                else if (salary > 5000 && age >= 60)
                {
                    epfEmployeePercentage = 0.055;
                    epfEmployerPercentage = 0.06;
                }
                else if (salary > 5000 && age <= 60)
                {
                    increaseValue = 100.00;
                    epfEmployeePercentage = 0.11;
                    epfEmployerPercentage = 0.12;
                }
                //else if (salary >= 20000) //if salary more than 20000  //temp
                //{
                //epfEmployeePercentage & epfEmployerPercentage, is depend on your percentage selected
                //}

                range = salary / 1000; //get the range; 0 = below 1000, 1 = 1000$, 10 = 10000$

                if (range > 1 && (range % 1) != 0) //for value > 1000
                {
                    range = (int)range * 1000;
                    bottomValue += range - 20.00;
                    topValue += range - 20.00;
                    if (topValue > 5000)
                    {
                        //bottomValue = bottomValue + 0.01;
                        topValue = topValue - 20.00 + 100.00;
                    }
                }
                else if (range > 0 && (range % 1) == 0) //for value like 1000,2000,5000
                {
                    epfEmployee = salary * epfEmployeePercentage;
                    epfEmployer = salary * epfEmployerPercentage;
                    exit = true;
                }

                while (exit == false)
                {
                    if (salary >= bottomValue && salary <= topValue)
                    {
                        epfEmployee = Math.Ceiling(topValue * epfEmployeePercentage);
                        epfEmployer = Math.Ceiling(topValue * epfEmployerPercentage);
                        exit = true;
                    }
                    bottomValue += increaseValue;
                    topValue += increaseValue;
                }
            }

            TextBox2.Text = epfEmployee.ToString();
            TextBox3.Text = epfEmployer.ToString();
        }
        private void EisCalculation()
        {
            //EIS calculation
            bool exit = false;
            double eisEmployer = 0.0, eisEmployee = 0.0;
            double salary = Convert.ToDouble(TextBox1.Text);
            double increaseValue = 100.00;
            double bottomValue = 200.01, topValue = 300.00;
            double eisPercentage = 0.002;

            if (salary >= 0.01 && salary <= 30.00)
                eisEmployer = eisEmployee = 0.05;
            else if (salary >= 30.01 && salary <= 50.00)
                eisEmployer = eisEmployee = 0.10;
            else if (salary >= 50.01 && salary <= 70.00)
                eisEmployer = eisEmployee = 0.15;
            else if (salary >= 70.01 && salary <= 100.00)
                eisEmployer = eisEmployee = 0.20;
            else if (salary >= 100.01 && salary <= 140.00)
                eisEmployer = eisEmployee = 0.25;
            else if (salary >= 140.01 && salary <= 200.00)
                eisEmployer = eisEmployee = 0.35;
            else if (salary >= 5000.01)
                eisEmployer = eisEmployee = 9.90;
            else
            {
                while (exit == false)
                {
                    if (salary >= bottomValue && salary <= topValue)
                    {
                        eisEmployer = eisEmployee = ((((int)bottomValue + topValue) / 2) * eisPercentage);
                        exit = true;
                    }
                    bottomValue += increaseValue;
                    topValue += increaseValue;
                }
            }
            TextBox2.Text = eisEmployee.ToString();
            TextBox3.Text = eisEmployer.ToString();
        }
        private void SocsoCalculation()
        {
            //socso calculation
            bool exit = false;
            double socsoEmployer = 0.0, socsoEmployee = 0.0;
            double salary = Convert.ToDouble(TextBox1.Text);
            double increaseValue = 100.00;
            double bottomValue = 200.01, topValue = 300.00;
            double socsoEmployeePercentage = 0.005, socsoEmployerPercentage = 0.0175;
            bool firstCategory = true;

            if (salary >= 0.01 && salary <= 30.00)
            {
                socsoEmployer = 0.40;
                socsoEmployee = 0.10;
            }
            else if (salary >= 30.01 && salary <= 50.00)
            {
                socsoEmployer = 0.70;
                socsoEmployee = 0.20;
            }
            else if (salary >= 50.01 && salary <= 70.00)
            {
                socsoEmployer = 1.10;
                socsoEmployee = 0.30;
            }
            else if (salary >= 70.01 && salary <= 100.00)
            {
                socsoEmployer = 1.50;
                socsoEmployee = 0.40;
            }
            else if (salary >= 100.01 && salary <= 140.00)
            {
                socsoEmployer = 2.10;
                socsoEmployee = 0.60;
            }
            else if (salary >= 140.01 && salary <= 200.00)
            {
                socsoEmployer = 2.95;
                socsoEmployee = 0.85;
            }
            else if (salary >= 5000.01)
            {
                socsoEmployer = 86.65;
                socsoEmployee = 24.75;
            }
            else
            {
                while (exit == false)
                {
                    if (salary >= bottomValue && salary <= topValue)
                    {
                        double dblTemp = ((((int)bottomValue + topValue) / 2) * socsoEmployerPercentage); // 6.375
                        int intTemp = (int)((((int)bottomValue + topValue) / 2) * socsoEmployerPercentage); // 6
                        int intTemp2 = 0;
                        double dblTemp2 = intTemp; // 6
                        dblTemp = dblTemp - intTemp; //6.375 - 6 = 0.375
                        dblTemp = (int)(dblTemp * 100); //0.375 = 37
                        intTemp2 = (int)dblTemp; //37
                        dblTemp = (int)(dblTemp % 10); //37 = 7
                        if (dblTemp > 5)
                            intTemp2 = intTemp2 - 2; //37 - 2 = 35 
                        else
                            intTemp2 = intTemp2 + 3;
                        dblTemp = (double)intTemp2 / 100; //35 / 100 = 0.35  
                        dblTemp2 += dblTemp; // 6 + 0.35 = 6.35

                        socsoEmployer = dblTemp2;
                        socsoEmployee = ((((int)bottomValue + topValue) / 2) * socsoEmployeePercentage);
                        exit = true;
                    }
                    bottomValue += increaseValue;
                    topValue += increaseValue;
                }
            }
            if (firstCategory == false)
            {
                socsoEmployer = socsoEmployer - socsoEmployee;
                socsoEmployee = 0.00;
            }
            TextBox2.Text = socsoEmployer.ToString();
            TextBox3.Text = socsoEmployee.ToString();
        }
        private static double RoundUp(double input, int places)
        {
            double multiplier = Math.Pow(10, Convert.ToDouble(places));
            return Math.Ceiling(input * multiplier) / multiplier;
        }
        private void TaxCalculation()
        {
            //tax, pcb or mtb
            //PCB for the current month = [ [ ( P – M ) R + B ] – ( Z + X ) ] / (n + 1)

            //https://blog.talenox.com/malaysia-tax-guide-how-do-i-calculate-pcb-mtd-part-2-of-3/            

            //if MTD for current month < 10$ , then MTD = 0
            //if MTD for current month >= 10$ , then MTD != 0
            //if MTD for current month after minus "zakat" < 10$ , then MTD != 0

            //Net PCB = PCB for the current month – zakat for the current month


            double P = 0.0, M = 0.0, R = 0.0, B = 0.0, Z = 0.0, X = 0.0;
            int n = 0;
            bool category3 = false;
            double pcbCurrentMonth = 0.0, netPCB = 0.0;
            double Y = 0.0, K = 0.0, Y1 = 0.0, K1 = 0.0, Y2 = 0.0, K2 = 0.0, D = 0.0, S = 0.0, Du = 0.0, Su = 0.0, Q = 0.0, LP = 0.0, LP1 = 0.0, Kt = 0.0;
            int C = 0;
            double zakat = 0.0;
            double salary = 0.0;
            bool selfDisabled = false, spouseDisable = false, married_spouseWork = true;
            bool additionalRemuneration = true;

            //calculation for P
            //[∑(Y - K) + (Y1 - K1) +[(Y2 - K2)n] ]-(D + S + Du + Su + QC +∑LP + LP1)
            //Y = sum up of monthly employee salary, eg. january = 4000, K=0  , then next month february, K = 4000, next month march K = 8000
            //K = sum up of monthly employee epf, eg. january = 440, K=0  , then next month february, K = 440, next month march K = 880
            //Y1 = employee salary
            //K1 = employee epf
            //Y2 = estimated next month employee salary
            //K2 = formula =>  ( 4000 - (K + K1 + Kt) ) / n      ; n = Remaining working month in a year ; Kt = ?????

            //Y take from database accumulated Y
            //K take from database accumulated K
            //LP take from database accumulated LP
            //LP1 take from textfield; LP1 = Other allowable deductions for current month ;  //maybe i will not do for this, shit of deduction

            string monthName = "January"; // temp
            n = 12 - DateTime.ParseExact(monthName, "MMMM", CultureInfo.CurrentCulture).Month;
            salary = 4000; //temp
            Y1 = salary;
            K1 = 440.0;// EpfCalculation(); //employee epf  //temp
            Y2 = salary;
            K2 = Math.Truncate(100 * ((4000 - (K + K1 + Kt)) / n)) / 100;
            D = 9000;
            Q = 2000;



            if (selfDisabled)
                Du = 6000;
            if (spouseDisable && !married_spouseWork)
            {
                S = 4000;
                Su = 5000;
            }
            else if (!married_spouseWork)
                S = 4000;

            P = ((Y - K) + (Y1 - K1) + ((Y2 - K2) * n)) - (D + S + Du + Su + (Q * C) + LP + LP1);


            //for Normal Remuneration
            if (P >= 5001 && P <= 20000.99)
            {
                M = 5000;
                R = 0.01;
                if (category3)
                    B = -800;
                else
                    B = -400;
            }
            else if (P >= 20001 && P <= 35000.99)
            {
                M = 20000;
                R = 0.03;
                if (category3)
                    B = -650;
                else
                    B = -250;
            }
            else if (P >= 35001 && P <= 50000.99)
            {
                M = 35000;
                R = 0.08;
                B = 600;
            }
            else if (P >= 50001 && P <= 70000.99)
            {
                M = 50000;
                R = 0.14;
                B = 1800;
            }
            else if (P >= 70001 && P <= 100000.99)
            {
                M = 70000;
                R = 0.21;
                B = 4600;
            }
            else if (P >= 100001 && P <= 250000.99)
            {
                M = 100000;
                R = 0.24;
                B = 10900;
            }
            else if (P >= 250001 && P <= 400000.99)
            {
                M = 250000;
                R = 0.245;
                B = 46900;
            }
            else if (P >= 400001 && P <= 600000.99)
            {
                M = 400000;
                R = 0.25;
                B = 836500;
            }
            else if (P >= 600001 && P <= 1000000.99)
            {
                M = 600000;
                R = 0.26;
                B = 133650;
            }
            else
            {
                M = 1000000;
                R = 0.28;
                B = 237650;
            }

            //"Z" take from database accumulated Z;    "X" take from database accumulated X;    "n" take month change to digit
            pcbCurrentMonth = ((((P - M) * R) + B) - (Z + X)) / (n + 1);
            if (pcbCurrentMonth < 10)
                pcbCurrentMonth = 0.0;
            netPCB = pcbCurrentMonth - zakat;
            if (netPCB <= 0)
                netPCB = 0;
            else if (additionalRemuneration == false)
            {
                netPCB = pcbRoundUp(netPCB);
            }

            //normal pcb done for above code


            if (additionalRemuneration)
            {
                //calculation for pcb(b)
                double pcb_B = 0.0;
                netPCB = Math.Truncate(100 * netPCB) / 100;
                pcb_B = (X) + (netPCB * (n + 1));

                //calculation for CS
                double CS = 0.0;
                double Yt = 1000.0; //Yt = any additional income eg. bonus, commissions, ...       //Kt = maybe is extra pay of EPF for current month
                P = ((Y - K) + (Y1 - K1) + ((Y2 - K2) * n) + (Yt - Kt)) - (D + S + Du + Su + (Q * C) + LP + LP1);
                if (P >= 5001 && P <= 20000.99)
                {
                    M = 5000;
                    R = 0.01;
                    if (category3)
                        B = -800;
                    else
                        B = -400;
                }
                else if (P >= 20001 && P <= 35000.99)
                {
                    M = 20000;
                    R = 0.03;
                    if (category3)
                        B = -650;
                    else
                        B = -250;
                }
                else if (P >= 35001 && P <= 50000.99)
                {
                    M = 35000;
                    R = 0.08;
                    B = 600;
                }
                else if (P >= 50001 && P <= 70000.99)
                {
                    M = 50000;
                    R = 0.14;
                    B = 1800;
                }
                else if (P >= 70001 && P <= 100000.99)
                {
                    M = 70000;
                    R = 0.21;
                    B = 4600;
                }
                else if (P >= 100001 && P <= 250000.99)
                {
                    M = 100000;
                    R = 0.24;
                    B = 10900;
                }
                else if (P >= 250001 && P <= 400000.99)
                {
                    M = 250000;
                    R = 0.245;
                    B = 46900;
                }
                else if (P >= 400001 && P <= 600000.99)
                {
                    M = 400000;
                    R = 0.25;
                    B = 836500;
                }
                else if (P >= 600001 && P <= 1000000.99)
                {
                    M = 600000;
                    R = 0.26;
                    B = 133650;
                }
                else
                {
                    M = 1000000;
                    R = 0.28;
                    B = 237650;
                }
                CS = ((P - M) * R) + B;

                //calculation for Additional Remuneration MTD 
                double pcb_C = 0.0;
                pcb_B = Math.Truncate(100 * pcb_B) / 100;
                pcb_C = CS - (pcb_B + Z);

                double totalPCB = 0.0;
                totalPCB = netPCB + pcb_C;

                totalPCB = pcbRoundUp(totalPCB);

            }
        }

        //RoundUp(189.182, 2);    189.182 => "189.19"
    }
}