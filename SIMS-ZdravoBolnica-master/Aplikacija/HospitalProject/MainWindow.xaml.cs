﻿using System;
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
using HospitalProject.Controller;
using HospitalProject.View;
using HospitalProject.View.DoctorView.Views;
using HospitalProject.View.Model;
using HospitalProject.View.PatientView;
using HospitalProject.View.PatientView.View;
using HospitalProject.View.WardenForms;

namespace HospitalProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainWindowViewModel mv = new MainWindowViewModel(this);
            this.DataContext = mv;
            mv.HarvestPassword += (sender, args) => args.Password = passwordBox1.Password;

        }


        
    }
}
