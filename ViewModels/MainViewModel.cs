using ClinicalTemplateReader;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESAPIX.Common;
using VMS.TPS.Common.Model.API;
using Prism.Commands;
using System.Collections.ObjectModel;
using TemplateBasedPlanningX.Models;
using System.Windows.Input;
using ESAPIX.Common.Args;

[assembly: ESAPIScript(IsWriteable = true)]

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

        AppComThread VMS = AppComThread.Instance;

        public MainViewModel()
        {
            
            GetTemplates();

            CreateTemplatePlanCommand = new DelegateCommand(() =>
            {
                var patientId = VMS.GetValue(sc =>
                {
                    return sc.Patient.Id;
                });
                var courseId = VMS.GetValue(sc =>
                {
                    return sc.Course.Id;
                });
                var structureSetId = VMS.GetValue(sc =>
                {
                    return sc.StructureSet.Id;
                });
                CreateTemplatePlan(patientId, courseId, structureSetId);
            });           
        }

        private void CreateTemplatePlan(string patientId, string courseId, string structureSetId)
        {

        }


        private void GetTemplates()
        {
            string server = "server";
            var clinicalProtols = new ClinicalTemplate(server);
            Templates = clinicalProtols.PlanTemplates;
        }

        public DelegateCommand CreateTemplatePlanCommand { get; set; }
        public DelegateCommand GetTemplatesCommand { get; set; }
        public DelegateCommand GetPrescriptionsCommand { get; set; }
        public List<PlanTemplate> Templates { get; set; } = new List<PlanTemplate>();
        public PlanTemplate SelectedTemplate { get; set; }
        public IEnumerable<Course> Courses { get; set; }
        public Course SelectedCourse { get; set; }
        public RTPrescription SelectedRx { get; set; }
    }
}
