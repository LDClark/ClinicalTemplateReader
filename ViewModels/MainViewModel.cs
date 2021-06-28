using ClinicalTemplateReader;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using VMS.TPS.Common.Model.API;
using ESAPIX.Interfaces;

//[assembly: ESAPIScript(IsWriteable = true)]

namespace ESAPX_StarterUI.ViewModels
{
    public class MainViewModel : BindableBase
    {
        private string _title = "TemplateBasedPlanning (ESAPIX)";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        //AppComThread VMS = AppComThread.Instance;

        private IESAPIService _es;
        public MainViewModel(IESAPIService es)
        {
            _es = es;

            GetTemplates();
            RxIdsInCourse = new List<string>();
            DosePerFraction = String.Empty;
            Fractions = String.Empty;

            _es.Execute(sac => sac.PatientChanged += Sac_PatientChanged);
            _es.Execute(sac => sac.CourseChanged += Sac_CourseChanged);
        }
        private void Sac_CourseChanged(Course course)
        {
            SelectedCourse = _es.GetValue(sac => sac.Course);
            IEnumerable<RTPrescription> rxs;
            if (SelectedCourse != null)
            {
                RxIdsInCourse = new List<string>();
                rxs = SelectedCourse.TreatmentPhases.FirstOrDefault().Prescriptions;
                foreach (var rx in rxs)
                {
                    RxIdsInCourse.Add(rx.Id);
                }
            }
        }
        private void Sac_PatientChanged(Patient patient)
        {
            SelectedPatientId = _es.GetValue(sac => sac.Patient?.Id);
        }
        private void CreateTemplatePlan()
        {

        }
        private void GetRx()
        {
            if (SelectedCourse != null)
            {
                var rx = _es.GetValue(sac => sac.Course?.TreatmentPhases.FirstOrDefault().Prescriptions.Where(x => x.Id == SelectedRx).FirstOrDefault());
                Fractions = rx.NumberOfFractions.ToString();
            }                     
        }
        private void GetTemplates()
        {
            string server = "server";
            var clinicalProtols = new ClinicalTemplate(server);
            Templates = clinicalProtols.PlanTemplates;
        }
        public List<PlanTemplate> Templates { get; set; } = new List<PlanTemplate>();
        public PlanTemplate SelectedTemplate { get; set; }
        public IEnumerable<Course> Courses { get; set; }

        private Course _selectedCourse;
        public Course SelectedCourse
        {
            get { return _selectedCourse; }
            set { SetProperty(ref _selectedCourse, value); }
        }

        private string _selectedPatientId;
        public string SelectedPatientId
        {
            get { return _selectedPatientId; }
            set { SetProperty(ref _selectedPatientId, value); }
        }
        private string _selectedRx;
        public string SelectedRx
        {
            get => _selectedRx;
            set
            {
                _selectedRx = value;
                GetRx();
            }
        }
        private List<string> _rxIdsInCourse;
        public List<string> RxIdsInCourse
        {
            get { return _rxIdsInCourse; }
            set { SetProperty(ref _rxIdsInCourse, value); }
        }
        private string _selectedCourseId;
        public string SelectedCourseId
        {
            get { return _selectedCourseId; }
            set { SetProperty(ref _selectedCourseId, value); }
        }
        private string _dosePerFraction;
        public string DosePerFraction
        {
            get { return _dosePerFraction; }
            set { SetProperty(ref _dosePerFraction, value); }
        }
        private string _fractions;
        public string Fractions
        {
            get { return _fractions; }
            set { SetProperty(ref _fractions, value); }
        }
    }
}
