﻿using Controller;
using HospitalProject.Controller;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Repository;
using Model;
using Service;
using HospitalProject.Service;
using HospitalProject.FileHandler;
using HospitalProject.Repository;
using HospitalProject.View.DoctorView.Model;

namespace HospitalProject
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static string _projectPath = System.Reflection.Assembly.GetExecutingAssembly().Location
            .Split(new string[] { "bin" }, StringSplitOptions.None)[0];
        private string MEETINGS_FILE = _projectPath + "\\Resources\\Data\\meetings.csv";
        private string EQUIPEMENT_FILE = _projectPath + "\\Resources\\Data\\equipement.csv";
        private string ROOM_RENOVATION_FILE = _projectPath + "\\Resources\\Data\\roomRenovations.csv";
        private string ROOM_FILE = _projectPath + "\\Resources\\Data\\rooms.csv";
        private string EQUIPMENT_RELOCATION_FILE = _projectPath + "\\Resources\\Data\\equipmentRelocations.csv";

        private const string CSV_DELIMITER = "|";
        private const string DATETIME_FORMAT = "MM/dd/yyyy HH:mm";
        private const string ONLY_DATE_FORMAT = "MM/dd/yyyy";
        
        
        public AllergiesController AllergiesController { get; set; }
        
        public MeetingsController MeetingsController { get; set; }
        public RoomRenovationController RenovationController { get; set; }

        public DoctorController DoctorController { get; set; }

        public PatientController PatientController { get; set; }

        public AppointmentController AppointmentController { get; set; }
        
        public EquipementController EquipementController { get; set; }

        public RoomControoler RoomController { get; set; }

        public AppointmentController AppointmentControllerPatient { get; set; }

        public AnamnesisController AnamnesisController { get; set; }

        public MedicalRecordController MedicalRecordController { get; set; }    

        public UserController UserController { get; set; }

        public PrescriptionController PrescriptionController { get; set; }

        public NotificationController NotificationController { get; set; }

        public CustomNotificationController CustomNotificationController { get; set; }

        public EquipmentRelocationController EquipmentRelocationController{ get; set; }

        public QuestionController QuestionController { get; set; }

        public SurveyController SurveyController { get; set; }

        public SurveyRealizationController SurveyRealizationController { get; set; }  
        
        public AnswerController AnswerController { get; set; }

        public MedicineReportController MedicineReportController { get; set; }  

        public VacationRequestController VacationRequestController { get; set; }

        public NoteController NoteController { get; set; }


        public App()
        {
            
           
            var _meetingsFileHandler = new MeetingsFileHandler(MEETINGS_FILE, CSV_DELIMITER, DATETIME_FORMAT);


            var _roomRenovationRepository = new RoomRenovationRepository();

            var _equipmentRelocationRepository = new EquipmentRelocationRepository();
            
            var _appointmentRepository = new AppointmentRepository(); 

            var _allergiesRepository = new AllergiesRepository();

            var _equipementRepository = new EquipementRepository( _allergiesRepository);

            var _doctorRepository = new DoctorRepository();
            
            var _roomRepository = new RoomRepository(ROOM_FILE, CSV_DELIMITER);

            var _medicalRecordRepository = new MedicalRecordRepository(_allergiesRepository);

            var _patientRepository = new PatientRepository(_medicalRecordRepository);

            var _anamnesisRepository = new AnamnesisRepository();

            var _prescriptionRepository = new PrescriptionRepository(_appointmentRepository);

            var _notificationRepository = new NotificationRepository(_prescriptionRepository);

            var _customNotificationRepository = new CustomNotificationRepository();

            var _userRepository = new UserRepository();

            var _questionRepository = new QuestionRepository();

            var _surveysRepository = new SurveyRepository(_questionRepository);

            var _answerRepository = new AnswerRepository();

            var _surveyRealizationRepository = new SurveyRealizationRepository(_patientRepository, _surveysRepository, _answerRepository);

            var _medicineReportRepository = new MedicineReportRepository(_equipementRepository, _doctorRepository);

            var _vacationRequestRepository = new VacationRequestRepository(_doctorRepository);

            var _noteRepository = new NoteRepository(_anamnesisRepository);

            var _allergiesService = new AllergiesService(_allergiesRepository, _medicalRecordRepository, _equipementRepository);
            
            var _equipementService = new EquipementService(_equipementRepository);

            var _roomService = new RoomService(_roomRepository);
            
            var _doctorService = new DoctorService(_doctorRepository, _roomService);
            
            var _meetingsRepository = new MeetingsRepository(_meetingsFileHandler, _doctorService);
            
            var _meetingsService = new MeetingsService(_meetingsRepository);
           
            var _patientService = new PatientService(_patientRepository);

            var _appointmentService = new AppointmentService(_appointmentRepository, _patientService, _doctorService, _roomService);

            var _anamnesisService = new AnamnesisService(_anamnesisRepository, _appointmentService);

            var _medicalRecordService = new MedicalRecordService(_allergiesService, _anamnesisService, _medicalRecordRepository, _patientService);

            var _userService = new UserService(_userRepository);

            var _prescriptionService = new PrescriptionService(_prescriptionRepository, _appointmentService, _equipementService, _medicalRecordService);

            var _roomRenovationService = new RoomRenovationService(_roomRenovationRepository,_appointmentService,_roomService);

            var _equipmentRelocationService = new EquipmentRelocationService(_equipmentRelocationRepository,_roomService);

            var _questionService = new QuestionService(_questionRepository);

            var _surveyService = new SurveyService(_surveysRepository);

            var _surveyRealizationService = new SurveyRealizationService(_surveyRealizationRepository);

            var _answerService = new AnswerService(_answerRepository);

            var _medicineReportService = new MedicineReportService(_medicineReportRepository);

            var _vacationRequestService = new VacationRequestService(_vacationRequestRepository);

            var _noteService = new NoteService(_noteRepository, _anamnesisService);

            var _notificationService = new NotificationService(_notificationRepository, _prescriptionService);

            var _customNotificationService = new CustomNotificationService(_customNotificationRepository);

            EquipmentRelocationController = new EquipmentRelocationController(_equipmentRelocationService);
            
            AllergiesController = new AllergiesController(_allergiesService);

            RenovationController = new RoomRenovationController(_roomRenovationService);

            EquipementController = new EquipementController(_equipementService);

            AppointmentController = new AppointmentController(_appointmentService);

            DoctorController = new DoctorController(_doctorService);

            PatientController = new PatientController(_patientService,_userService, _medicalRecordService, _appointmentService);

            RoomController = new RoomControoler(_roomService);

            AnamnesisController = new AnamnesisController(_anamnesisService);

            AllergiesController = new AllergiesController(_allergiesService);
            
            MeetingsController = new MeetingsController(_meetingsService);

            MedicalRecordController = new MedicalRecordController(_medicalRecordService);

            UserController = new UserController(_userService);

            PrescriptionController = new PrescriptionController(_prescriptionService);

            NotificationController = new NotificationController(_notificationService);

            CustomNotificationController = new CustomNotificationController(_customNotificationService);

            QuestionController = new QuestionController(_questionService);

            SurveyController = new SurveyController(_surveyService);

            SurveyRealizationController = new SurveyRealizationController(_surveyRealizationService);

            AnswerController = new AnswerController(_answerService);

            VacationRequestController = new VacationRequestController(_vacationRequestService);

            MedicineReportController = new MedicineReportController(_medicineReportService);

            NoteController = new NoteController(_noteService);

        }

        public void ChangeLanguage(string currLang)
        {
            if (currLang.Equals("en-US"))
            {
                TranslationManager.Instance.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            }
            else if (currLang.Equals("es-ES"))
            {
                TranslationManager.Instance.CurrentCulture = new System.Globalization.CultureInfo("es-ES");
            }
            else
            {
                TranslationManager.Instance.CurrentCulture = new System.Globalization.CultureInfo("sr-LATN");
            }
        }

    }
}
