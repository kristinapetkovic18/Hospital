﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Syncfusion.XPS;

namespace HospitalProject.View.DoctorView.Model
{
    public class TranslationManager : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private static readonly TranslationManager instance = new TranslationManager();

        public static TranslationManager Instance
        {
            get { return instance; }
        }

        private readonly ResourceManager resManager = Resource.ResourceManager;
        private CultureInfo currentCulture = null;

        public string this[string key]
        {
            get
            {
                string retVal = this.resManager.GetString(key, this.currentCulture);
                return retVal;
            }
        }

        public CultureInfo CurrentCulture
        {
            get
            {
                return this.currentCulture;
            }
            set
            {
                if (this.currentCulture != value)
                {
                    this.currentCulture = value;
                    var @event = this.PropertyChanged;
                    if (@event != null)
                    {
                        @event.Invoke(this, new PropertyChangedEventArgs(string.Empty));
                    }
                }
            }
        }


    }

    public class LocExtension : Binding
    {
        public LocExtension(string name) : base("[" + name + "]")
        {
            this.Mode = BindingMode.OneWay;
            this.Source = TranslationManager.Instance;
        }
    }
}
