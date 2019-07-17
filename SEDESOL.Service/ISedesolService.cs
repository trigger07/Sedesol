using SEDESOL.DataEntities.DTO;
using SEDESOL.DataEntities.IntegrationObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace SEDESOL.Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ISedesolService" in both code and config file together.
    [ServiceContract]
    public interface ISedesolService
    {
        #region Capture
        [OperationContract]
        List<CaptureDTO> GetCaptures();

        [OperationContract]
        List<CaptureDTO> GetCapturesSearch(int userTypeId, int userId, int pState, int pSoupK, int pStatus);

        [OperationContract]
        List<CaptureDTO> GetCapturesFilter(int userTypeId, int userId);

        [OperationContract]
        void ActivateCapture(int id);

        [OperationContract]
        void DeactivateCapture(int id);

        [OperationContract]
        CaptureDTO SaveCapture(CaptureDTO capture);

        [OperationContract]
        bool UpdateCapture(CaptureDTO capture);

        [OperationContract]
        CaptureDTO GetCaptureById(int id);

        [OperationContract]
        int SaveCaptureImage(CaptureImageDTO dto);

        [OperationContract]
        string DeleteCaptureImage(int FileId);

        [OperationContract]
        List<CaptureImageDTO> GetImageByCaptureId(int pCaptureId);

        [OperationContract]
        List<InspectionCodeDTO> GetCodesByUserId(int pUserId);

        [OperationContract]
        string SendToApproval(CaptureApprovalDTO dto);

        #endregion

        #region Params
        [OperationContract]
        CaptureParam GetParamCapture(int userTypeId, int userId);

        [OperationContract]
        string EditStatus(int idStatus, int idCapture, int idUserType);

        [OperationContract]
        List<StateDTO> GetStates();

        [OperationContract]
        List<StateDTO> GetStatesFilter(int userTypeId, int userId);

        [OperationContract]
        List<StatusDTO> GetStatus();

        [OperationContract]
        List<YearDTO> GetYears();

        [OperationContract]
        List<MonthDTO> GetMonths();

        [OperationContract]
        void UpdateState(StateDTO dto);

        [OperationContract]
        void UpdateStatus(StatusDTO dto);

        [OperationContract]
        void UpdateYear(YearDTO dto);

        [OperationContract]
        void UpdateMonth(MonthDTO dto);

        [OperationContract]
        void ActivateStatus(int id);

        [OperationContract]
        void DeactivateStatus(int id);

        [OperationContract]
        void ActivateState(int id);

        [OperationContract]
        void DeactivateState(int id);

        [OperationContract]
        void ActivateYear(int id);

        [OperationContract]
        void DeactivateYear(int id);

        [OperationContract]
        void ActivateMonth(int id);

        [OperationContract]
        void DeactivateMonth(int id);

        [OperationContract]
        CodeParam GetParamCode(int pUserId);

        [OperationContract]
        string GenerateCode(int pUserId);

        [OperationContract]
        List<StateDTO> GetActiveStates();

        [OperationContract]
        List<ConditionDTO> GetActiveCondition();

        [OperationContract]
        List<RegionDTO> GetActiveRegions();
        #endregion

        #region Attendance
        [OperationContract]
        AttendanceDTO SaveAttendance(AttendanceDTO dto);

        [OperationContract]
        AttendanceDTO GetAttendanceById(int id);

        [OperationContract]
        string DeleteAttendance(int id);

        [OperationContract]
        List<AttendanceDTO> GetDataForAc(string term);
        #endregion

        #region Reports
        [OperationContract]
        string GetAttendanceReport(int pCaptureId);

        [OperationContract]
        string GetCaptureReport(List<FilterDTO> filters);

        [OperationContract]
        List<SoupKitchenDTO> GetSoupKitchenByUser(int userTypeId, int userId);
        #endregion

        #region Login
        [OperationContract]
        UserDTO ValidateUser(UserValidationRequest req);
        #endregion

        #region SoupKitchen
        [OperationContract]
        SoupKitchenDTO SaveSoupKitchen(SoupKitchenDTO dto);

        [OperationContract]
        List<SoupKitchenDTO> GetSoupKitchenAll();

        [OperationContract]
        List<SoupKitchenDTO> GetSoupKitchenToAssignUser(int id);

        [OperationContract]
        List<SoupKitchenDTO> GetSKAssignedUser(int id);

        [OperationContract]
        SoupKitchenDTO GetSoupKitchenById(int id);

        [OperationContract]
        void Deactivate(int id);

        [OperationContract]
        void Activate(int id);

        [OperationContract]
        List<SkUserTypeDTOcs> GetUserTypeBySKId(int skId);

        [OperationContract]
        string DeleteUserTypeSk(int id);
        #endregion

        #region User
        [OperationContract]
        UserDTO SaveUser(UserDTO dto);

        [OperationContract]
        List<UserDTO> GetUserAll();

        [OperationContract]
        UserDTO GetUserById(int id);

        [OperationContract]
        void DeactivateUser(int id);

        [OperationContract]
        void ActivateUser(int id);

        [OperationContract]
        List<UserTypeDTO> GetUserTypeAll();

        [OperationContract]
        List<UserTypeDTO> GetUserTypeApproval();

        [OperationContract]
        SkUserTypeDTOcs SaveUserTypeSK(SkUserTypeDTOcs dto);

        [OperationContract]
        UserSoupKitchen SaveUserSoupKitchen(UserSoupKitchen dto);

        [OperationContract]
        string DeleteUserSoupKitchen(int idSk, int idUser);
        #endregion

        #region InspectionCode
        [OperationContract]
        InspectionCodeDTO SaveInspectionCode(InspectionCodeDTO dto);

        [OperationContract]
        List<GencodeDayDTO> GetGenerationCodeDayAll();

        [OperationContract]
        GencodeDayDTO SaveGencodeDay(GencodeDayDTO dto);

        [OperationContract]
        GencodeDayDTO GetGenerationCodeDayById(int Id);

        [OperationContract]
        GencodeDayParam GetParamGen();
        #endregion

        #region CaptureApproval
        [OperationContract]
        List<CaptureApprovalDTO> GetApprovalByCaptureId(int captureId);
        #endregion

        #region Product
        [OperationContract]
        List<ProductDTO> GetProductAll();

        [OperationContract]
        ProductDTO GetProductById(int idProduct);

        [OperationContract]
        ProductDTO SaveProduct(ProductDTO prodDto);
        #endregion

        #region Rations
        [OperationContract]
        RationModel GetRationSearch(int pYear, int pMonth);

        [OperationContract]
        string SaveRations(string data);

        [OperationContract]
        CouponModel GetCouponData(int pIdRation);

        [OperationContract]
        List<CouponModel> GetCouponDataByState(int pIdState, int pIdYear, int pIdMonth);

        [OperationContract]
        string GetDataExcel(int pIdState, int pIdMonth, int pIdYear);
        #endregion

        #region Configuration
        [OperationContract]
        List<ConfigurationDTO> GetConfiguration(string value);
        #endregion
    }
}
