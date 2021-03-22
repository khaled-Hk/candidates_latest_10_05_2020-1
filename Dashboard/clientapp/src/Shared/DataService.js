import axios from 'axios';

axios.defaults.headers.common['X-XSRF-TOKEN'] = document.cookie.split('=')[1];
//const baseUrl = 'http://localhost:4810/Api';

export default {
    IsLogin() {
        return axios.get(`/Security/IsLogin`);
    },
    GetProfiles(pageNo, pageSize) {
        return axios.get(`/Api/Admin/Profile/Get?pageno=${pageNo}&pagesize=${pageSize}`);
    },
    GetAllProfiles() {
        return axios.get(`/Api/Admin/Profile/GetAllProfiles`);
    },
    GetRuningProfile() {
        return axios.get(`/Api/Admin/Profile/GetRuningProfile`);
    },
    GetActiveProfile() {
        return axios.get(`/Api/Admin/Profile/GetActiveProfile`);
    },
    AddProfiles(Profile) {
        return axios.post(`/Api/Admin/Profile/Add`, Profile);
    },
    ActivateProfile(ProfileId) {
        return axios.post(`/Api/Admin/Profile/${ProfileId}/ActivateProfile`);
    },
    DeActivateProfile(ProfileId) {
        return axios.post(`/Api/Admin/Profile/${ProfileId}/DeActivateProfile`);
    },
    PlayProfile(ProfileId) {
        return axios.post(`/Api/Admin/Profile/${ProfileId}/PlayProfile`);
    },
    PauseProfile(ProfileId) {
        return axios.post(`/Api/Admin/Profile/${ProfileId}/PauseProfile`);
    },
    // ***************************** Regions ****************************
    GetRegions(pageNo, pageSize) {
        return axios.get(`/Api/Admin/Regions/Get?pageno=${pageNo}&pagesize=${pageSize}`);
    },
    AddRegions(Region) {
        return axios.post(`/Api/Admin/Regions/Add`, Region);
    },
    DeleteRegion(RegionId) {
        return axios.delete(`/Api/Admin/Regions/${RegionId}/Delete`);
    },
    DisableRegion(RegionId) {
        return axios.put(`/Api/Admin/Regions/${RegionId}/Disable`);
    },
    EnableRegion(RegionId) {
        return axios.put(`/Api/Admin/Regions/${RegionId}/Enable`);
    },
    GetAllRegions() {
        return axios.get(`/Api/Admin/Regions/GetRegions`);
    },
    // ************************ Entities **************************
    GetEntities(pageNo, pageSize) {

        return axios.get(`/Api/Admin/Entities/Get?pageno=${pageNo}&pagesize=${pageSize}`);
    },
    AddEnitity(Entity) {
        return axios.post('/Api/Admin/Entities/Add', Entity);
    },
    AddEntityUser(User) {
        return axios.post('/Api/Admin/Entities/AddEntityUser', User);
    },
    getEntityUser(id) {
        return axios.get(`/api/Admin/Entities/getEntityUser/${id}`);
    },
    DeactivateEntityUser(UserId) {
        return axios.post(`/Api/Admin/Entities/${UserId}/Deactivate`);
    },

    ActivateEntityUser(UserId) {
        return axios.post(`/Api/Admin/Entities/${UserId}/Activate`);
    },

    delteEntityUser(UserId) {
        return axios.post(`/Api/Admin/Entities/${UserId}/delteUser`);
    },
    // ************************ Representatives **************************
    GetRepresentativesByEntityId(id) {
        return axios.get(`/api/Admin/Representatives/GetRepresentativesByEntityId/${id}`);
    },

    AddRepresentatives(Representatives) {
        return axios.post('/Api/Admin/Representatives/Add', Representatives);
    },
    // ************************ User Info **************************
    CheckLoginStatus() {
        return axios.post('/security/checkloginstatus');
    },

    getUser(pageNo, pageSize) {
        return axios.get(`/Api/Admin/User/getUser?pageNo=${pageNo}&pagesize=${pageSize}`);
    },

    AddUser(User) {
        return axios.post('/Api/Admin/User/AddUser', User);
    },

    EditUser(User) {
        return axios.post('/Api/Admin/User/EditUser', User);
    },

    DeactivateUser(UserId) {
        return axios.post(`/Api/Admin/User/${UserId}/Deactivate`);
    },

    ActivateUser(UserId) {
        return axios.post(`/Api/Admin/User/${UserId}/Activate`);
    },

    delteUser(UserId) {
        return axios.post(`/Api/Admin/User/${UserId}/delteUser`);
    },

    UploadImage(obj) {
        return axios.post('/Api/Admin/User/UploadImage', obj);
    },

    EditUsersProfile(User) {
        return axios.post('/Api/Admin/User/EditUsersProfile', User);
    },

    ChangePassword(userPassword) {
        return axios.post(`/Security/ChangePassword`, userPassword);
    },




    // ************************ Constituencies **************************

    GetConstituencies() {
        return axios.get(`/Api/Admin/Constituencies/Get`);
    },
    AddConstituency(constituency) {
        return axios.post(`/api/Admin/Constituencies/Add`, constituency);
    },
    DeleteConstituency(constituencyId) {
        return axios.delete(`/api/Admin/Constituencies/${constituencyId}/Delete`);
    },
    GetConstituenciesBasedOn(RegionId) {
        return axios.get(`/Api/Admin/Constituencies/${RegionId}/Region`);
    },
    GetConstituencyBasedOn(constituencyId) {
        return axios.get(`/Api/Admin/Constituencies/Get/${constituencyId}`);
    },

    GetConstituenciesDetalsChairs(ConstituencyId) {
        return axios.get(`/Api/Admin/ConstituencyDetails/${ConstituencyId}/Chairs`);
    },
    GetAConstituencyBasedOn(regionId) {
        return axios.get(`/Api/Admin/Constituencies/Get/${regionId}/Region`);
    },
    GetConstituencyPagination(pageNo, pageSize) {
        return axios.get(`/Api/Admin/Constituencies/Get?pageNo=${pageNo}&pageSize=${pageSize}`);
    },
    UpdateConstituency(constituency) {
        return axios.put(`/Api/Admin/Constituencies/`, constituency);
    },
    // ************************ ConstituencyDetails **************************
    AddConstituencyDetails(constituencyDetials) {
        return axios.post(`/api/Admin/ConstituencyDetails`, constituencyDetials);
    },
    GetConstituencyDetails() {
        return axios.get(`/api/Admin/ConstituencyDetails/All`);
    },
    DeleteConstituencyDetail(constituencyId) {
        return axios.delete(`/api/Admin/ConstituencyDetails/${constituencyId}`);
    },
    GetConstituencyDetailsBasedOn(constituencyDetailsId) {
        return axios.get(`/api/Admin/ConstituencyDetails/Get/${constituencyDetailsId}`);
    },
    GetAllConstituencyDetailsBasedOn(constituencyId) {
        return axios.get(`/api/Admin/ConstituencyDetails/Get/Constituency/${constituencyId}`);
    },
    GetConstituencyDetailsPagination(pageNo, pageSize) {
        return axios.get(`/api/Admin/ConstituencyDetails?pageNo=${pageNo}&pageSize=${pageSize}`);
    },
    UpdateConstituencyDetail(constituencyDetail) {
        return axios.put(`/api/Admin/ConstituencyDetails`, constituencyDetail);
    },

    // ************************ Centers **************************
    AddCenter(center) {
        return axios.post(`/api/Admin/Centers`, center);
    },
    GetCentersPagination(pageNo, pageSize) {
        return axios.get(`/api/Admin/Centers/Get?pageNo=${pageNo}&pageSize=${pageSize}`);
    },
    DeleteCenter(centerId) {
        return axios.delete(`/api/Admin/Centers/${centerId}`);
    },
    GetCenter(centerId) {
        return axios.get(`/api/Admin/Centers/Get/${centerId}`);
    },
    GetAllCenters() {
        return axios.get(`/Api/Admin/Centers/Get/All`);
    },
    GetCentersBasedOn(constituencyDetialId) {
        return axios.get(`/Api/Admin/Centers/GetCentersBasedOn/${constituencyDetialId}`);
    },
    UpdateCenter(center) {
        return axios.put(`/api/Admin/Centers`, center);
    },
    // ************************ Stations **************************
    GetStations(centerId, pageNo, pageSize) {
        return axios.get(`/api/Admin/Stations/Get?centerId=${centerId}&pageNo=${pageNo}&pageSize=${pageSize}`);
    },
    CreateStations(stations) {
        return axios.post(`/api/Admin/Stations/Add`, stations);
    },
    DeleteStation(stationId) {
        return axios.delete(`/api/Admin/Stations/Delete/${stationId}`);
    },
    GetStationBasedOn(stationId) {
        return axios.get(`/api/Admin/Stations/Get/${stationId}`);
    },
    UpdateStation(station) {
        return axios.put(`/api/Admin/Stations/Update`, station);
    },
    // ************************ Candidates **************************
    GetCandidate(CandidateId) {
        return axios.get(`/api/Admin/Candidates/Get/${CandidateId}`);
    },
    GetCandidatesByEntityId(id) {
        return axios.get(`/api/Admin/Candidates/${id}/Entity`);
    },
    AddCandidateToEntity(candidate) {
        return axios.post(`/api/Admin/Candidates/AddCandidateToEntity`, candidate);
    },
    GetCandidates(pageNo, pageSize,SubConstituencyId) {
        return axios.get(`/api/Admin/Candidates/Get?pageNo=${pageNo}&pageSize=${pageSize}&SubConstituencyId=${SubConstituencyId}`);
    },
    RegisterCandidateLevelOne(nationalId) {

        return axios.get(`/api/Admin/Candidates/NationalId/${nationalId}`);
    },
    RegisterCandidateContact(object) {

        return axios.post(`/api/Admin/Candidates/SendVerifyCode`, object);
    },
    VerifyPhone(object) {
        return axios.post(`/api/Admin/Candidates/VerifyPhone`, object);

    },
    UploadCandidateData(object) {
        return axios.post(`/api/Admin/Candidates/Data`, object);

    },
    UploadFiles(object) {
        return axios.post(`/api/Admin/Candidates/UploadDocuments`, object);

    },
    UpdateCandidate(candidate) {
        return axios.put(`/api/Admin/Candidates`, candidate);
    },
    UploadCandidateAttachments(candidate) {
        return axios.post(`/api/Admin/Candidates/Attachments`, candidate);
    },
    GetCandidateInfo(NationalId) {
        return axios.get(`/api/Admin/Candidates/Complete/${NationalId}`);
    },
    AddCandidateUser(User) {
        return axios.post('/Api/Admin/Candidates/Add/User', User);
    },
    GetCandidateUser(candidateId) {
        return axios.get(`/Api/Admin/Candidates/GetUser/${candidateId}`);
    },
    GetCandidateIdByNid(Nid) {
       
        return axios.get(`/api/Admin/Candidates?nationalId=${Nid}`);
    },
    //DeleteCompany(CompanyId) {
    //    axios.defaults.headers.common['Authorization'] = 'Bearer ' + document.querySelector('meta[name="api-token"]').getAttribute('content');
    //    return axios.post(`/Api/Admin/Companies/${CompanyId}/delete`);
    //},
    //GetCountries(pageNo, pageSize) {
    //    axios.defaults.headers.common['Authorization'] = 'Bearer ' + document.querySelector('meta[name="api-token"]').getAttribute('content');
    //    return axios.get(`/Api/Admin/Countries/Get?pageno=${pageNo}&pagesize=${pageSize}`);
    //},
    //GetAllCountries() {
    //    axios.defaults.headers.common['Authorization'] = 'Bearer ' + document.querySelector('meta[name="api-token"]').getAttribute('content');
    //    return axios.get(`/Api/Admin/Countries/GetAllCountries`);
    //},

    //AddCountries(Country) {
    //    axios.defaults.headers.common['Authorization'] = 'Bearer ' + document.querySelector('meta[name="api-token"]').getAttribute('content');
    //    return axios.post(`/Api/Admin/Countries/Add`, Country);
    //},
    //DeleteCountry(CountryId) {
    //    axios.defaults.headers.common['Authorization'] = 'Bearer ' + document.querySelector('meta[name="api-token"]').getAttribute('content');
    //    return axios.post(`/Api/Admin/Countries/${CountryId}/delete`);
    //},

    //GetCities(pageNo, pageSize, CountryId) {
    //    axios.defaults.headers.common['Authorization'] = 'Bearer ' + document.querySelector('meta[name="api-token"]').getAttribute('content');
    //    return axios.get(`/Api/Admin/Cities/Get?pageno=${pageNo}&pagesize=${pageSize}&CountryId=${CountryId}`);
    //},
    ////GetCityByCountryId(CountryId) {
    ////    axios.defaults.headers.common['Authorization'] = 'Bearer ' + document.querySelector('meta[name="api-token"]').getAttribute('content');
    ////    return axios.get(`/Api/Admin/Cities/GetCityByCountryId?CountryId=${CountryId}`);
    ////},
    //AddCities(Cities) {
    //    axios.defaults.headers.common['Authorization'] = 'Bearer ' + document.querySelector('meta[name="api-token"]').getAttribute('content');
    //    return axios.post(`/Api/admin/Cities/Add`, Cities);
    //},
    //DeleteCity(CityId) {
    //    axios.defaults.headers.common['Authorization'] = 'Bearer ' + document.querySelector('meta[name="api-token"]').getAttribute('content');
    //    return axios.post(`/Api/Admin/Cities/${CityId}/delete`);
    //},

    GetBranches(pageNo, pageSize) {
        return axios.get(`/Api/Admin/Branches/Get?pageno=${pageNo}&pagesize=${pageSize}`);
    },
    DeleteBranche(BranchId) {
        return axios.delete(`/Api/Admin/Branches/${BranchId}/Delete`);
    },
    AddBranch(Branch) {
        return axios.post(`/Api/Admin/Branches/Add`, Branch);
    },
    //************************ Offices **************************

    GetAllOffices() {
        return axios.get(`/Api/Admin/Offices/GetAllOffices`);
    },
    GetOffice(pageNo, pageSize) {
        return axios.get(`/Api/Admin/Offices/Get?pageno=${pageNo}&pagesize=${pageSize}`);
    },
    deleteOffice(id) {
        return axios.post(`/Api/Admin/Offices/${id}/Delete`);
    },
    AddOffice(OfficeData) {
        return axios.post(`/Api/Admin/Offices/Add`, OfficeData);
    },
    //************************ Represenatives Of Candidates **************************

    AddCandidateRepresentatives(candidateRepresentatives) {
        return axios.post(`/Api/Admin/Representatives/Candidate/Add`, candidateRepresentatives);
    },
    GetCandidateRepresentatives(candidateIs) {
        return axios.get(`/Api/Admin/Representatives/GetRepresentativesBy/${candidateIs}`);
    },
    AddCandidateRepresentative(form) {
        return axios.post(`/Api/Admin/Representatives/Candidate/Add`, form);
    },

    //************************ Chairs **************************
    GetChairs(pageNo, pageSize) {
        return axios.get(`/Api/Admin/Chairs/Get?pageno=${pageNo}&pagesize=${pageSize}`);
    },

    GetChairsDetails(pageNo, pageSize) {
        return axios.get(`/Api/Admin/Chairs/Get/Details?pageno=${pageNo}&pagesize=${pageSize}`);
    },

    deleteChairs(id) {
        return axios.post(`/Api/Admin/Chairs/${id}/delete`);
    },

    deleteChairsDetails(id) {
        return axios.post(`/Api/Admin/Chairs/${id}/Details/Delete`);
    },

    AddChairs(form) {
        return axios.post(`/Api/Admin/Chairs/Add`, form);
    },

    AddChairsDetails(form) {
        return axios.post(`/Api/Admin/Chairs/Add/Details`, form);
    },

    //************************ Endorsements **************************
    GetEndorsements(CandidateId, pageNo, pageSize) {
        return axios.get(`/Api/Admin/Endorsements?candidateId=${CandidateId}&pageNo=${pageNo}&pageSize=${pageSize}`);
    },
    GetEndorsementsByNid(Nid) {
        
        return axios.get(`/Api/Admin/Endorsements/Get?nationalId=${Nid}`);
    },
    AddEndorsement(obj) {
        return axios.post(`/Api/Admin/Endorsements`, obj);
    },
    AddNewEndorsement(obj) {
        return axios.post(`/Api/Admin/Endorsements/Add`, obj);
    },

    //************************ Statistics **************************
    GetStatistics() {
        return axios.get(`/Api/Admin/Statistics`);
    },



}