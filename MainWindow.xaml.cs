using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using System.Globalization;

namespace Lazy_Romeo
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        #region MonthsOverall
        public static int MonthsOverall(DateTime Dday)
        {
            int months_overall = 0;
            DateTime t1 = Dday;
            while (DateTime.Compare(DateTime.Now, t1) > 0)
            {
                months_overall++;
                t1 = Dday.AddMonths(months_overall);
            }
            return months_overall - 1;
        }
        #endregion

        #region DaysOverall
        public static int DaysOverall(int indexMday, int indexMmonth, int indexMyear)
        {
            int days_overall = 0;
            if (DateTime.Now.Year - indexMyear > 0)
            {
                // Add days starting from next to D-Day full month to next New Year, e.g. 10.01.2005 - 12.31.2005
                for (int m = 12; m > indexMmonth; m--)
                    days_overall += DateTime.DaysInMonth(indexMyear, m);
                // Add days starting from recent New Year to full month before current date, e.g. 01.01.2019 - 08.31.2019
                for (int m = 1; m < DateTime.Now.Month; m++)
                    days_overall += DateTime.DaysInMonth(DateTime.Now.Year, m);
                // Add days of current month, e.g. 09.01.2019 - 09.10.2019
                days_overall += DateTime.Now.Day;
                // Add days of D-Day month, e.g. 09.10.2005 - 09.30.2005
                days_overall += (DateTime.DaysInMonth(indexMyear, indexMmonth) - indexMday);
            }

            if (DateTime.Now.Year - indexMyear > 1)
            {   // Add days from every year between current year and year of D-Day (not including), e.g. 01.01.2006 - 12.31.2018
                for (int y = 1; y < DateTime.Now.Year - indexMyear; y++)
                    if (DateTime.IsLeapYear(DateTime.Now.Year - y) is true)
                        days_overall += 366;
                    else days_overall += 365;
            }

            if ((DateTime.Now.Year - indexMyear == 0) & (DateTime.Now.Month - indexMmonth > 0))
            {
                // Add days of current month, e.g. 09.01.2019 - 09.10.2019
                days_overall += DateTime.Now.Day;
                // Add days of D-Day month, e.g. 09.10.2005 - 09.30.2005
                days_overall += (DateTime.DaysInMonth(indexMyear, indexMmonth) - indexMday);
                // Add days of full months between current date and D-Day since it is same year
                for (int m = indexMmonth + 1; m < DateTime.Now.Month; m++)
                    days_overall += DateTime.DaysInMonth(DateTime.Now.Year, m);
            }

            if ((DateTime.Now.Year - indexMyear == 0) & (DateTime.Now.Month - indexMmonth == 0))
            {
                //Add days between D-Day and current date since it is same month
                days_overall += (DateTime.Now.Day - indexMday);
            }
            return days_overall;
        }
        #endregion

        private void ProfileListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
           //ProfileDetailsWindow.
        }

        public MainWindow()
        {
            InitializeComponent();
            DDayCalendar.Visibility = Visibility.Hidden;
            Label1.Visibility = Visibility.Hidden;
        }

        private void Close_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            LRWindow.Close();
        }

        private void Open_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            ProfileDetailsWindow w2 = new ProfileDetailsWindow();
            w2.Show();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }


        private void Date_Picker_Click(object sender, SelectionChangedEventArgs e)
        {
            DateTime? firstdate = firstdatepick.SelectedDate;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DaysTextBox.Clear();
            WeeksTextBox.Clear();
            MonthsTextBox.Clear();
            DateTime? firstdate = firstdatepick.SelectedDate;
            DDayCalendar.DisplayDate = firstdate.Value.Date;
            DDayCalendar.SelectedDate = firstdate.Value.Date;
            DDayCalendar.Visibility = Visibility.Visible;
            Label1.Visibility = Visibility.Visible;
            Label3.Content = "";
            Label4.Content = "";
            Label5.Content = "";

            int day_index = DaysOverall(firstdate.Value.Date.Day, firstdate.Value.Date.Month, firstdate.Value.Date.Year);

            //     DAYS anniversary counter
            Label3.Content = "It's " + day_index + " days since VID today";
            if (day_index % 100 == 0 | day_index % 1000 == 0) DaysTextBox.AppendText(" TODAY is " + day_index + " days since D-Day" + "\n");
            for (int i = 1, q = 0; q < 3; i++)
            {
                day_index = DaysOverall(firstdate.Value.Date.Day, firstdate.Value.Date.Month, firstdate.Value.Date.Year) + i;
                if (day_index % 100 == 0 | day_index % 1000 == 0)
                {
                    DateTime anniversary_days = DateTime.Now.AddDays(i);
                    if (day_index % 1000 == 0)
                    {

                    }
                    DaysTextBox.AppendText(anniversary_days.ToString(" MM.dd.yyyy") + " it will be " + day_index + " days since D-Day" + "\n");
                    q++;
                }
            }

            //     WEEKS annyversary counter   
            day_index = DaysOverall(firstdate.Value.Date.Day, firstdate.Value.Date.Month, firstdate.Value.Date.Year);
            int week_index = day_index/7;
            if (day_index % 7 == 0) Label4.Content = "It's " + week_index + " weeks since VID today";
            if (week_index % 10 == 0 | week_index % 100 == 0)
            {

                WeeksTextBox.Foreground = Brushes.Red;
                WeeksTextBox.AppendText(" TODAY is " + week_index + " weeks since D-Day" + "\n");
                WeeksTextBox.Foreground = Brushes.Black;
            }

            for (int i = week_index, q = 0; q < 3; i++)
            {
                if ((week_index % 10 == 0 | week_index % 100 == 0) & (DateTime.Compare(firstdate.Value.Date.AddDays(i * 7),DateTime.Now)>0))
                {
                    DateTime anniversary_weeks = firstdate.Value.Date.AddDays(i*7);
                    if (week_index % 100 == 0)
                    {

                    }
                    WeeksTextBox.AppendText(anniversary_weeks.ToString(" MM.dd.yyyy") + " it will be " + week_index + " weeks since D-Day" + "\n");
                    q++;
                }
                week_index++;

            }

            //     MONTHS anniversary counter
            int month_index = MonthsOverall(firstdate.Value.Date);
            if (firstdate.Value.Date.Day == DateTime.Now.Date.Day)
            {
                Label5.Content = "It's " + month_index + " months since VID today";
            }
            for (int i = month_index, q = 0; i < 1000 & q < 3; i++)
            {
                if ((i % 10 == 0 | i % 100 == 0 | i % 1000 == 0) & i > 0)
                {
                    DateTime anniversary_months = firstdate.Value.Date.AddMonths(i);
                    if (i % 10 == 0 | i % 100 == 0)
                    {

                    }
                    MonthsTextBox.AppendText(anniversary_months.ToString(" MM.dd.yyyy") + " it will be " + i + " months since D-Day" + "\n");

                    q++;
                }
            }


        }
    }
}
