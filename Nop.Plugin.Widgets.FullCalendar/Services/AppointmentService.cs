using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Localization;
using Nop.Core.Domain.Messages;
using Nop.Core.Domain.Stores;
using Nop.Core.Events;
using Nop.Data;
using Nop.Plugin.Widgets.FullCalendar.Domain;
using Nop.Services.Events;
using Nop.Services.Localization;
using Nop.Services.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.FullCalendar.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IRepository<Appointment> _newAppointmentRepository;
        private readonly IEventPublisher _eventPublisher;
        private readonly ILocalizationService _localizationService;
        private readonly IStoreContext _storeContext;
        private readonly IMessageTemplateService _messageTemplateService;
        private readonly IWorkflowMessageService _workflowMessageService;
        private readonly IMessageTokenProvider _messageTokenProvider;
        private readonly IEmailAccountService _emailAccountService;
        private readonly EmailAccountSettings _emailAccountSettings;
        private IWorkContext _workContext;
        private LocalizationSettings _localizationSettings;

        public AppointmentService(IRepository<Appointment> newAppointmentRepository,
            IEventPublisher eventPublisher, 
            IStoreContext storeContext,
            IMessageTemplateService messageTemplateService, 
            IWorkflowMessageService workflowMessageService,
            IMessageTokenProvider messageTokenProvider,
            IEmailAccountService emailAccountService,
            EmailAccountSettings emailAccountSettings,
            IWorkContext workContext,
            ILocalizationService localizationService,
            LocalizationSettings localizationSettings)
        {
            _newAppointmentRepository = newAppointmentRepository;
            _eventPublisher = eventPublisher;
            _storeContext = storeContext;
            _messageTemplateService = messageTemplateService;
            _workflowMessageService = workflowMessageService;
            _messageTokenProvider = messageTokenProvider;
            _emailAccountService = emailAccountService;
            _emailAccountSettings = emailAccountSettings;
            _workContext = workContext;
            _localizationService = localizationService;
            _localizationSettings = localizationSettings;
        }

        public virtual async Task InsertAppointment(Appointment appointment)
        {
            await _newAppointmentRepository.InsertAsync(appointment);
            await _eventPublisher.EntityInsertedAsync(appointment);
            var currentCustomer = await _workContext.GetCurrentCustomerAsync();
            await SendCustomerAppointmentNotificationMessage(currentCustomer, _localizationSettings.DefaultAdminLanguageId, appointment);
        }

        public virtual async Task SendCustomerAppointmentNotificationMessage(Customer customer, int languageId, Appointment appointment)
        {
            if (customer == null)
                throw new ArgumentNullException(nameof(customer));

            var store = await _storeContext.GetCurrentStoreAsync();
            var messageTemplates = await _messageTemplateService.GetMessageTemplatesByNameAsync("Appointment.New", store.Id);
            var messageTemplate = messageTemplates.FirstOrDefault();

            if (messageTemplate == null)
                return;

            var emailAccount = await GetEmailAccountOfMessageTemplateAsync(messageTemplate, languageId);
            var tokens = new List<Token>();
            await _messageTokenProvider.AddStoreTokensAsync(tokens, store, emailAccount);
           await  _messageTokenProvider.AddCustomerTokensAsync(tokens, customer);
            // Add appointment specific tokens here
            tokens.Add(new Token("Appointment.ContactNumber", appointment.ContactNumber));
            tokens.Add(new Token("Appointment.Date", appointment.AppointmentDateTimeUTC.ToString("D")));
            tokens.Add(new Token("Appointment.Reason", appointment.AppointmentReason));

            await _eventPublisher.MessageTokensAddedAsync(messageTemplate, tokens);

            var toEmail = emailAccount.Email;
            var toName = emailAccount.DisplayName;
            await _workflowMessageService.SendNotificationAsync(messageTemplate, emailAccount, languageId, tokens, toEmail, toName);
        }

        protected virtual async Task<EmailAccount> GetEmailAccountOfMessageTemplateAsync(MessageTemplate messageTemplate, int languageId)
        {
            var emailAccountId = await _localizationService.GetLocalizedAsync(messageTemplate, mt => mt.EmailAccountId, languageId);
            if (emailAccountId == 0)
                emailAccountId = messageTemplate.EmailAccountId;

            var emailAccount = await _emailAccountService.GetEmailAccountByIdAsync(emailAccountId)
                ?? await _emailAccountService.GetEmailAccountByIdAsync(_emailAccountSettings.DefaultEmailAccountId)
                ?? (await _emailAccountService.GetAllEmailAccountsAsync()).FirstOrDefault();

            if (emailAccount == null)
                throw new Exception("No email account could be loaded");

            return emailAccount;
        }


        async Task<IList<Appointment>> IAppointmentService.GetAllAppointments(bool showHidden = false, int storeId = 0)
        {
            return await _newAppointmentRepository.Table.ToListAsync();
        }
    }
}
