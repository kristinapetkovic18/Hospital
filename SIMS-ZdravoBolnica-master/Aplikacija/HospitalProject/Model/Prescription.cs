﻿using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalProject.Model
{
    public class Prescription : ViewModelBase
    {
        private int id;
        private Appointment appointment;
        private DateTime startDate;
        private DateTime endDate;
        private int interval;
        private Equipement medicine;
        private TimeOnly startTime;
        private string description;

        // Constructor that is called when creating a new object

        public Prescription() { }

        public Prescription(Appointment appointment, DateTime startDate, DateTime endDate, int interval, string description, Equipement medicine)
        {
            Appointment = appointment;
            Medicine = medicine;
            SetFieldsForConstructor(startDate, endDate, interval, description);
        }

        public Prescription(int id, int appointmentId, DateTime startDate, DateTime endDate, int interval, string description, int medicineId)
        {
            Id = id;
            SetIds(appointmentId, medicineId);
            SetFieldsForConstructor(startDate, endDate, interval, description);
        }

        private void SetFieldsForConstructor(DateTime startDate, DateTime endDate, int interval, string description)
        {
            StartDate = startDate;
            EndDate = endDate;
            Interval = interval;
            Description = description;
        }

        private void SetIds(int appointmentId, int medicineId)
        {
            InstantiateObjects();
            Appointment.Id = appointmentId;
            Medicine.Id = medicineId;
        }

        private void InstantiateObjects()
        {
            Appointment = new Appointment();
            Medicine = new Equipement();
        }

        public int Interval
        {
            get
            {
                return interval;
            }
            set
            {
                interval = value;
                OnPropertyChanged(nameof(Interval));
            }
        }

        public int Id
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
                OnPropertyChanged(nameof(Id));
            }
        }

        public DateTime StartDate
        {
            get
            {
                return startDate;
            }
            set
            {
                startDate = value;
                OnPropertyChanged(nameof(StartDate));
            }
        }

        public DateTime EndDate
        {
            get
            {
                return endDate;
            }
            set
            {
                endDate = value;
                OnPropertyChanged(nameof(EndDate));
            }
        }

        public TimeOnly StartTime
        {
            get
            {
                return startTime;
            }
            set
            {
                startTime = value;
                OnPropertyChanged(nameof(StartTime));
            }
        }

        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        public Appointment Appointment
        {
            get
            {
                return appointment;
            }
            set
            {
                appointment = value;
                OnPropertyChanged(nameof(Appointment));
            }
        }

        public Equipement Medicine
        {
            get
            {
                return medicine;
            }
            set
            {
                medicine = value;
                OnPropertyChanged(nameof(Medicine));
            }
        }
    }
}
