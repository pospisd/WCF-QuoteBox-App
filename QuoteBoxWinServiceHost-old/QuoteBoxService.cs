using System;
using System.ServiceModel;
using System.ServiceProcess;

namespace QuoteBoxWinServiceHost
{

    /// <summary>
    /// Install this project into Managed Windows Services.
    /// This is useful for services that must remain running once started.
    /// Services can be started and stopped using the service control manager.
    /// </summary>
    public partial class QuoteBoxService : ServiceBase
    {

        private ServiceHost serviceHost;

        public QuoteBoxService()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// OnStart...
        /// </summary>
        /// <param name="args"></param>
        protected override void OnStart(string[] args)
        {
            // Just to be really safe.
            if (serviceHost != null)
            {
                serviceHost.Close();
                serviceHost = null;
            }

            // Create the host.
            serviceHost = new ServiceHost(typeof(QuoteBoxWinServiceHost.QuoteBoxService));

            // Open the host.
            serviceHost.Open();
        }

        /// <summary>
        /// OnStop...
        /// </summary>
        protected override void OnStop()
        {
            // Shut down the host.
            if (serviceHost != null)
            {
                serviceHost.Close();
                serviceHost = null;
            }
        }
    }
}