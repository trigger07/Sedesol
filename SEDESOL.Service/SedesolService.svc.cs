using SEDESOL.BusinessLogic;
using SEDESOL.DataEntities.DTO;
using SEDESOL.DataEntities.IntegrationObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace SEDESOL.Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "SedesolService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select SedesolService.svc or SedesolService.svc.cs at the Solution Explorer and start debugging.
    public class SedesolService : ISedesolService
    {
        #region Capture
        public List<CaptureDTO> GetCaptures()
        {
            CaptureDAL dal = new CaptureDAL();
            return dal.GetCaptures();
        }

        public List<CaptureDTO> GetCapturesFilter(int userTypeId, int userId)
        {
            CaptureDAL dal = new CaptureDAL();
            return dal.GetCaptures(userTypeId, userId);
        }

        public List<CaptureDTO> GetCapturesSearch(int userTypeId, int userId, int pState, int pSoupK, int pStatus)
        {
            CaptureDAL dal = new CaptureDAL();
            return dal.GetCapturesSearch(userTypeId, userId, pState, pSoupK, pStatus);
        }

        public void ActivateCapture(int id)
        {
            CaptureDAL dal = new CaptureDAL();
            dal.Activate(id);
        }

        public void DeactivateCapture(int id)
        {
            CaptureDAL dal = new CaptureDAL();
            dal.Activate(id);
        }

        public CaptureDTO SaveCapture(CaptureDTO capture)
        {
            CaptureDAL dal = new CaptureDAL();
            return dal.Save(capture);
        }

        public bool UpdateCapture(CaptureDTO capture)
        {
            CaptureDAL dal = new CaptureDAL();
            return dal.Update(capture);
        }

        public CaptureDTO GetCaptureById(int id)
        {
            CaptureDAL dal = new CaptureDAL();
            return dal.GetCaptureById(id);
        }

        public CaptureParam GetParamCapture(int userTypeId, int userId)
        {
            SoapKitchenDAL sKDal = new SoapKitchenDAL();
            CaptureParam capModel = new CaptureParam();
            ParamDAL parDal = new ParamDAL();

            capModel.Months = parDal.GetMonths();
            capModel.Years = parDal.GetYears();
            capModel.SoapKitchens = sKDal.GetSoapKitchenByUser(userTypeId, userId);

            return capModel;
        }

        public string EditStatus(int idStatus, int idCapture, int idUserType)
        {
            CaptureDAL dal = new CaptureDAL();
            return dal.EditStatus(idStatus, idCapture, idUserType);
        }

        public int SaveCaptureImage(CaptureImageDTO dto)
        {
            CaptureImageDAL dal = new CaptureImageDAL();
            return dal.Save(dto);
        }

        public string DeleteCaptureImage(int FileId)
        {
            CaptureImageDAL d = new CaptureImageDAL();
            return d.Delete(FileId);
        }
        public List<CaptureImageDTO> GetImageByCaptureId(int pCaptureId)
        {
            CaptureImageDAL dal = new CaptureImageDAL();
            return dal.GetImageByCaptureId(pCaptureId);
        }

        #endregion

        #region Attendance
        public AttendanceDTO SaveAttendance(AttendanceDTO dto)
        {
            AttendanceDAL dal = new AttendanceDAL();
            return dal.Save(dto);
        }

        public AttendanceDTO GetAttendanceById(int id)
        {
            AttendanceDAL dal = new AttendanceDAL();
            return dal.GetAttendanceById(id);
        }

        public string DeleteAttendance(int id)
        {
            AttendanceDAL dal = new AttendanceDAL();
            return dal.Delete(id);
        }

        public List<AttendanceDTO> GetDataForAc(string term)
        {
            AttendanceDAL dal = new AttendanceDAL();
            return dal.GetDataForAc(term);
        }
        #endregion

        #region Reports
        public string GetAttendanceReport(int pCaptureId)
        {
            ReportDAL dal = new ReportDAL();
            return dal.GetAttendanceReport(pCaptureId);
        }

        public string GetCaptureReport(List<FilterDTO> filters)
        {
            ReportDAL dal = new ReportDAL();
            return dal.GetCaptureReport(filters);
        }
        #endregion

        #region Params
        public List<StateDTO> GetStates()
        {
            ParamDAL dal = new ParamDAL();
            return dal.GetStates();
        }

        public List<StateDTO> GetStatesFilter(int userTypeId, int userId)
        {
            ParamDAL dal = new ParamDAL();
            return dal.GetStatesFilter(userTypeId, userId);
        }

        public List<MonthDTO> GetMonths()
        {
            ParamDAL dal = new ParamDAL();
            return dal.GetMonths();
        }

        public List<StatusDTO> GetStatus()
        {
            ParamDAL dal = new ParamDAL();
            return dal.GetStatus();
        }

        public List<YearDTO> GetYears()
        {
            ParamDAL dal = new ParamDAL();
            return dal.GetYears();
        }

        public void ActivateStatus(int id)
        {
            ParamDAL dal = new ParamDAL();
            dal.ActivateStatus(id);
        }

        public void DeactivateStatus(int id)
        {
            ParamDAL dal = new ParamDAL();
            dal.DeactivateStatus(id);
        }

        public void ActivateState(int id)
        {
            ParamDAL dal = new ParamDAL();
            dal.ActivateState(id);
        }

        public void DeactivateState(int id)
        {
            ParamDAL dal = new ParamDAL();
            dal.DeactivateState(id);
        }

        public void ActivateYear(int id)
        {
            ParamDAL dal = new ParamDAL();
            dal.ActivateYear(id);
        }

        public void DeactivateYear(int id)
        {
            ParamDAL dal = new ParamDAL();
            dal.DeactivateYear(id);
        }

        public void ActivateMonth(int id)
        {
            ParamDAL dal = new ParamDAL();
            dal.ActivateMonth(id);
        }

        public void DeactivateMonth(int id)
        {
            ParamDAL dal = new ParamDAL();
            dal.DeactivateMonth(id);
        }

        public List<SoupKitchenDTO> GetSoupKitchenByUser(int userTypeId, int userId)
        {
            SoapKitchenDAL dal = new SoapKitchenDAL();
            return dal.GetSoapKitchenByUser(userTypeId, userId);
        }

        public List<StateDTO> GetActiveStates()
        {
            ParamDAL dal = new ParamDAL();
            return dal.GetActiveStates();
        }

        public List<ConditionDTO> GetActiveCondition()
        {
            ParamDAL dal = new ParamDAL();
            return dal.GetActiveCondition();
        }

        public List<RegionDTO> GetActiveRegions()
        {
            RegionDAL dal = new RegionDAL();
            return dal.GetActiveRegions();
        }
        #endregion

        #region Login
        public UserDTO ValidateUser(UserValidationRequest req)
        {
            LoginDAL dal = new LoginDAL();
            return dal.ValidateUser(req);
        }

        public void UpdateState(StateDTO dto)
        {
            ParamDAL dal = new ParamDAL();
            dal.UpdateState(dto);
        }

        public void UpdateStatus(StatusDTO dto)
        {
            ParamDAL dal = new ParamDAL();
            dal.UpdateStatus(dto);
        }

        public void UpdateYear(YearDTO dto)
        {
            ParamDAL dal = new ParamDAL();
            dal.UpdateYear(dto);
        }

        public void UpdateMonth(MonthDTO dto)
        {
            ParamDAL dal = new ParamDAL();
            dal.UpdateMonth(dto);
        }
        #endregion

        #region SoupKitchen
        public SoupKitchenDTO SaveSoupKitchen(SoupKitchenDTO dto)
        {
            SoapKitchenDAL dal = new SoapKitchenDAL();
            return dal.Save(dto);
        }

        public List<SoupKitchenDTO> GetSoupKitchenAll()
        {
            SoapKitchenDAL dal = new SoapKitchenDAL();
            return dal.GetSoupKitchenAll();
        }

        public List<SoupKitchenDTO> GetSoupKitchenToAssignUser(int id)
        {
            SoapKitchenDAL dal = new SoapKitchenDAL();
            return dal.GetSoupKitchenToAssignUser(id);
        }

        public List<SoupKitchenDTO> GetSKAssignedUser(int id)
        {
            SoapKitchenDAL dal = new SoapKitchenDAL();
            return dal.GetSKAssignedUser(id);
        }

        public SoupKitchenDTO GetSoupKitchenById(int id)
        {
            SoapKitchenDAL dal = new SoapKitchenDAL();
            return dal.GetSoupKitchenById(id);
        }

        public void Deactivate(int id)
        {
            SoapKitchenDAL dal = new SoapKitchenDAL();
            dal.Deactivate(id);
        }

        public void Activate(int id)
        {
            SoapKitchenDAL dal = new SoapKitchenDAL();
            dal.Activate(id);
        }

        public string DeleteUserTypeSk(int id)
        {
            SoapKitchenDAL dal = new SoapKitchenDAL();
            return dal.DeleteUserTypeSk(id);
        }
        #endregion

        #region User
        public UserDTO SaveUser(UserDTO dto)
        {
            UserDAL dal = new UserDAL();
            return dal.Save(dto);
        }

        public List<UserDTO> GetUserAll()
        {
            UserDAL dal = new UserDAL();
            return dal.GetUserAll();
        }

        public UserDTO GetUserById(int id)
        {
            UserDAL dal = new UserDAL();
            return dal.GetUserById(id);
        }

        public void DeactivateUser(int id)
        {
            UserDAL dal = new UserDAL();
            dal.Deactivate(id);
        }

        public void ActivateUser(int id)
        {
            UserDAL dal = new UserDAL();
            dal.Activate(id);
        }

        public List<UserTypeDTO> GetUserTypeAll()
        {
            UserDAL dao = new UserDAL();
            return dao.GetUserTypeAll();
        }

        public List<UserTypeDTO> GetUserTypeApproval()
        {
            UserDAL dao = new UserDAL();
            return dao.GetUserTypeApproval();
        }

        public List<SkUserTypeDTOcs> GetUserTypeBySKId(int skId)
        {
            SoapKitchenDAL dal = new SoapKitchenDAL();
            return dal.GetUserTypeBySKId(skId);
        }

        public SkUserTypeDTOcs SaveUserTypeSK(SkUserTypeDTOcs dto)
        {
            UserDAL dal = new UserDAL();
            return dal.SaveUserTypeSK(dto);
        }

        public UserSoupKitchen SaveUserSoupKitchen(UserSoupKitchen dto)
        {
            UserDAL dal = new UserDAL();
            return dal.SaveUserSoupKitchen(dto);
        }


        public string DeleteUserSoupKitchen(int idSk, int idUser)
        {
            UserDAL dal = new UserDAL();
            return dal.DeleteUserSoupKitchen(idSk, idUser);
        }
        #endregion

        #region InspectionCode
        public string GenerateCode(int pUserId)
        {
            InspectionCodeDAL dal = new InspectionCodeDAL();
            return dal.GenerateCode(pUserId);
        }

        public List<GencodeDayDTO> GetGenerationCodeDayAll()
        {
            GencodeDayDAL dal = new GencodeDayDAL();
            return dal.GetGenerationCodeDayAll();
        }

        public List<InspectionCodeDTO> GetCodesByUserId(int pUserId)
        {
            InspectionCodeDAL dal = new InspectionCodeDAL();
            return dal.GetCodesByUserId(pUserId);
        }

        public InspectionCodeDTO SaveInspectionCode(InspectionCodeDTO dto)
        {
            InspectionCodeDAL dal = new InspectionCodeDAL();
            return dal.Save(dto);
        }

        public GencodeDayDTO SaveGencodeDay(GencodeDayDTO dto)
        {
            GencodeDayDAL dal = new GencodeDayDAL();
            return dal.Save(dto);
        }

        public GencodeDayDTO GetGenerationCodeDayById(int Id)
        {
            GencodeDayDAL dal = new GencodeDayDAL();
            return dal.GetGenerationCodeDayById(Id);
        }

        public CodeParam GetParamCode(int pUserId)
        {
            CodeParam capModel = new CodeParam();
            ParamDAL parDal = new ParamDAL();
            InspectionCodeDAL ins = new InspectionCodeDAL();

            capModel.ListCode = ins.GetCodesByUserId(pUserId);
            capModel.Months = parDal.GetMonths();
            capModel.Years = parDal.GetYears();

            return capModel;
        }

        public GencodeDayParam GetParamGen()
        {
            GencodeDayParam capModel = new GencodeDayParam();

            ParamDAL parDal = new ParamDAL();
            GencodeDayDAL ins = new GencodeDayDAL();
            capModel.Day = null;
            capModel.listGencodeDayDTO = ins.GetGenerationCodeDayAll();
            capModel.Months = parDal.GetMonths();
            capModel.Years = parDal.GetYears();

            return capModel;
        }
        #endregion

        #region CaptureApproval
        public List<CaptureApprovalDTO> GetApprovalByCaptureId(int captureId)
        {
            CaptureApprovalDAL dal = new CaptureApprovalDAL();
            return dal.GetApprovalByCaptureId(captureId);
        }

        public string SendToApproval(CaptureApprovalDTO dto)
        {
            CaptureApprovalDAL dal = new CaptureApprovalDAL();
            return dal.SaveApproval(dto);
        }
        #endregion

        #region Product
        public List<ProductDTO> GetProductAll()
        {
            ProductDAL dal = new ProductDAL();
            return dal.GetProductAll();
        }

        public ProductDTO GetProductById(int idProduct)
        {
            ProductDAL dal = new ProductDAL();
            return dal.GetProductById(idProduct);
        }

        public ProductDTO SaveProduct(ProductDTO prodDto)
        {
            ProductDAL dal = new ProductDAL();
            return dal.Save(prodDto);
        }
        #endregion

        #region Rations
        public RationModel GetRationSearch(int pYear, int pMonth)
        {
            RationDAL dal = new RationDAL();
            ParamDAL paramdal = new ParamDAL();
            RationModel model = new RationModel();

            model.ListMonth = paramdal.GetMonths();
            model.ListYear = paramdal.GetYears();
            model.ListRation = dal.GetRationSearch(pYear, pMonth);
            return model;
        }

        public string SaveRations(string data)
        {
            DataTable dt = (DataTable)DTOSerializer.Deserialize(data, typeof(DataTable));
            RationDAL dal = new RationDAL();

            return dal.SaveRations(dt);
        }

        public CouponModel GetCouponData(int pIdRation)
        {
            RationDAL dal = new RationDAL();
            return dal.GetCouponData(pIdRation);
        }

        public List<CouponModel> GetCouponDataByState(int pIdState, int pIdYear, int pIdMonth)
        {
            RationDAL dal = new RationDAL();
            return dal.GetCouponDataByState(pIdState, pIdYear, pIdMonth);
        }

        public string GetDataExcel(int pIdState, int pIdMonth, int pIdYear)
        {
            RationDAL dal = new RationDAL();
            return dal.GetExcel(pIdState, pIdMonth, pIdYear);
        }
        #endregion

        #region Configuration
        public List<ConfigurationDTO> GetConfiguration(string value)
        {
            ConfigurationDAL dal = new ConfigurationDAL();
            return dal.GetConfiguration(value);
        }
        #endregion
    }
}
