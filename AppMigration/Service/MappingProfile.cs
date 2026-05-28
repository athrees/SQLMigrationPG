using AppMigration.Msql.Entity;
using AppMigration.Postgree.Entities;
using AutoMapper;

namespace AppMigration.Service
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<SQL_ApplicationUser, ApplicationUser>();
            CreateMap<SQL_CLIENT, CLIENT>().ForMember(dest => dest.CLIENT_ID, opt => opt.Ignore());
            CreateMap<SQL_CLIENT_ACCOUNT, CLIENT_ACCOUNT>().ForMember(dest => dest.CLIENT_ACCOUNT_ID, opt => opt.Ignore());
            CreateMap<SQL_CLIENT_FEE, CLIENT_FEE>().ForMember(dest => dest.CLIENT_FEE_ID, opt => opt.Ignore());

            CreateMap<SQL_CUSTOMER, CUSTOMER>().ForMember(dest => dest.CUSTOMER_ID, opt => opt.Ignore());
            CreateMap<SQL_BRANCH, BRANCH>().ForMember(dest => dest.BRANCH_ID, opt => opt.Ignore());
            CreateMap<SQL_NOTARIS, NOTARIS>().ForMember(dest => dest.NOTARIS_ID, opt => opt.Ignore());
            CreateMap<SQL_PNBP, PNBP>().ForMember(dest => dest.PNBP_ID, opt => opt.Ignore());
            CreateMap<SQL_INVOICE, INVOICE>();
            CreateMap<SQL_INVOICE_HISTORY, INVOICE_HISTORY>().ForMember(dest => dest.HISTORY_ID, opt => opt.Ignore());

            CreateMap<SQL_DOCUMENTS, DOCUMENTS>();
            CreateMap<SQL_DOCUMENTS_CONFIG, DOCUMENTS_CONFIG>().ForMember(dest => dest.CONFIG_ID, opt => opt.Ignore());
            CreateMap<SQL_DOCUMENTS_FILE, DOCUMENTS_FILE>().ForMember(dest => dest.FILE_ID, opt => opt.Ignore());
            CreateMap<SQL_DOCUMENTS_IMPORT, DOCUMENTS_IMPORT>().ForMember(dest => dest.ID, opt => opt.Ignore());
            CreateMap<SQL_HISTORY_CERTIFICATE, HISTORY_CERTIFICATE>().ForMember(dest => dest.ID, opt => opt.Ignore());

            CreateMap<SQL_AHUACCOUNT, AHUACCOUNT>().ForMember(dest => dest.AHU_ID, opt => opt.Ignore());

            CreateMap<SQL_POSTS, POSTS>().ForMember(dest => dest.POST_ID, opt => opt.Ignore());
            CreateMap<SQL_ACCESS, ACCESS>();
            CreateMap<SQL_ACCESSROLES, ACCESSROLES>().ForMember(dest => dest.ACCESS_ROLE_ID, opt => opt.Ignore());


        }
    }
}
