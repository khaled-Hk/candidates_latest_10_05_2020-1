import axios from 'axios';

axios.defaults.headers.common['X-CSRF-TOKEN'] = document.cookie.split("=")[1];

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
        return axios.get(`/Api/Admin/Constituencies/GetAllConstituencies`);
    },
    AddConstituency(constituency) {
        return axios.post(`/api/Admin/Constituencies/CreateConstituency`, constituency);
    },
    DeleteConstituency(constituencyId) {
        return axios.delete(`/api/Admin/Constituencies/DeleteConstituency/${constituencyId}`);
    },
    GetConstituenciesBasedOn(RegionId) {
        return axios.get(`/Api/Admin/Constituencies/GetConstituenciesBasedOn/${RegionId}`);
    },
    GetConstituencyBasedOn(constituencyId) {
        return axios.get(`/Api/Admin/Constituencies/GetConstituency/${constituencyId}`);
    },

    GetConstituenciesDetalsChairs(ConstituencyId) {
        return axios.get(`/Api/Admin/Constituencies/GetConstituency/${ConstituencyId}`);
    },
    GetAConstituencyBasedOn(regionId) {
        return axios.get(`/Api/Admin/Constituencies/GetAConstituency/${regionId}`);
    },
    GetConstituencyPagination(pageNo, pageSize) {
        return axios.get(`/Api/Admin/Constituencies/Get?pageNo=${pageNo}&pageSize=${pageSize}`);
    },
    UpdateConstituency(constituency) {
        return axios.put(`/Api/Admin/Constituencies/UpdateConstituency/`, constituency);
    },
    // ************************ ConstituencyDetails **************************
    AddConstituencyDetails(constituencyDetials) {
        return axios.post(`/api/Admin/ConstituencyDetails/CreateConstituencyDetails`, constituencyDetials);
    },
    GetConstituencyDetails() {
        return axios.get(`/api/Admin/ConstituencyDetails/GetAllConstituencyDetails`);
    },
    DeleteConstituencyDetail(constituencyId) {
        return axios.delete(`/api/Admin/ConstituencyDetails/DeleteConstituencyDetails/${constituencyId}`);
    },
    GetConstituencyDetailsBasedOn(constituencyDetailsId) {
        return axios.get(`/api/Admin/ConstituencyDetails/GetConstituencyDetail/${constituencyDetailsId}`);
    },
    GetAllConstituencyDetailsBasedOn(constituencyId) {
        return axios.get(`/api/Admin/ConstituencyDetails/GetConstituencyDetails/${constituencyId}`);
    },
    GetConstituencyDetailsPagination(pageNo, pageSize) {
        return axios.get(`/api/Admin/ConstituencyDetails/ConstituencyDetailsPagination?pageNo=${pageNo}&pageSize=${pageSize}`);
    },
    UpdateConstituencyDetail(constituencyDetail) {
        return axios.put(`/api/Admin/ConstituencyDetails/UpdateConstituencyDetails`, constituencyDetail);
    },

    // ************************ Centers **************************
    AddCenter(center) {
        return axios.post(`/api/Admin/Centers/CreateCenter`, center);
    },
    GetCentersPagination(pageNo, pageSize) {
        return axios.get(`/api/Admin/Centers/CenterPagination?pageNo=${pageNo}&pageSize=${pageSize}`);
    },
    DeleteCenter(centerId) {
        return axios.delete(`/api/Admin/Centers/DeleteCenter/${centerId}`);
    },
    GetCenter(centerId) {
        return axios.get(`/api/Admin/Centers/GetCenter/${centerId}`);
    },
    GetAllCenters() {
        return axios.get(`/Api/Admin/Centers/GetCenters`);
    },
    GetCentersBasedOn(constituencyDetialId) {
        return axios.get(`/Api/Admin/Centers/GetCentersBasedOn/${constituencyDetialId}`);
    },
    UpdateCenter(center) {
        return axios.put(`/api/Admin/Centers/UpdateCenter`, center);
    },
    // ************************ Stations **************************
    GetStations(centerId, pageNo, pageSize) {
        return axios.get(`/api/Admin/Stations/GetStations?centerId=${centerId}&pageNo=${pageNo}&pageSize=${pageSize}`);
    },
    CreateStations(stations) {
        return axios.post(`/api/Admin/Stations/CreateStations`, stations);
    },
    DeleteStation(stationId) {
        return axios.delete(`/api/Admin/Stations/DeleteStation/${stationId}`);
    },
    GetStationBasedOn(stationId) {
        return axios.get(`/api/Admin/Stations/GetStationBasedOn/${stationId}`);
    },
    UpdateStation(station) {
        return axios.put(`/api/Admin/Stations/UpdateStation`, station);
    },
    // ************************ Candidates **************************
    GetCandidate(CandidateId) {
        return axios.get(`/api/Admin/Candidates/GetCandidate/${CandidateId}`);
    },
    GetCandidatesByEntityId(id) {
        return axios.get(`/api/Admin/Candidates/GetCandidatesByEntityId/${id}`);
    },
    AddCandidateToEntity(candidate) {
        return axios.post(`/api/Admin/Candidates/AddCandidateToEntity`, candidate);
    },
    GetCandidates(pageNo, pageSize) {
        return axios.get(`/api/Admin/Candidates/GetCandidates?pageNo=${pageNo}&pageSize=${pageSize}`);
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
        return axios.post(`/api/Admin/Candidates/CandidateData`, object);

    },
    UploadFiles(object) {
        return axios.post(`/api/Admin/Candidates/UploadDocuments`, object);

    },
    UpdateCandidate(candidate) {
        return axios.put(`/api/Admin/Candidates/UpdateCandidate`, candidate);
    },
    UploadCandidateAttachments(candidate) {
        return axios.post(`/api/Admin/Candidates/CandidateAttachments`, candidate);
    },
    GetCandidateInfo(NationalId) {
        return axios.get(`/api/Admin/Candidates/CompleteRegistration/${NationalId}`);
    },
    AddCandidateUser(User) {
        return axios.post('/Api/Admin/Candidates/AddCandidateUser', User);
    },
    GetCandidateUser(candidateId) {
        return axios.get(`/Api/Admin/Candidates/GetCandidateUser/${candidateId}`);
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
        return axios.post(`/Api/Admin/Offices/${id}/deleteOffice`);
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


    //************************ Chairs **************************
    GetChairs(pageNo, pageSize) {
        return axios.get(`/Api/Admin/Chairs/Get?pageno=${pageNo}&pagesize=${pageSize}`);
    },

    GetChairsDetails(pageNo, pageSize) {
        return axios.get(`/Api/Admin/Chairs/GetChairsDetails?pageno=${pageNo}&pagesize=${pageSize}`);
    },

    deleteChairs(id) {
        return axios.post(`/Api/Admin/Chairs/${id}/delete`);
    },

    deleteChairsDetails(id) {
        return axios.post(`/Api/Admin/Chairs/${id}/deleteChairsDetails`);
    },

    AddChairs(form) {
        return axios.post(`/Api/Admin/Chairs/Add`, form);
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

   




    //Login(loginName, password, secretNo) {
    //    return axios.post(baseUrl + '/security/login', { loginName, password, secretNo });
    //},
    //Logout() {
    //    return axios.post(baseUrl + '/security/logout');
    //},    
    //CheckLoginStatus() {
    //    return axios.post('/security/checkloginstatus');
    //},  
    ////********************* Student Service *****************************
    //GetStudents(pageNo, pageSize, UserType, CompanyId) {
    //    axios.defaults.headers.common['Authorization'] = 'Bearer ' + document.querySelector('meta[name="api-token"]').getAttribute('content');
    //    return axios.get(baseUrl + `/Admin/Students/Get?pageno=${pageNo}&pagesize=${pageSize}&UserType=${UserType}&CompanyId=${CompanyId}`);
    //},
    //DeleteStudent(StudentId) {
    //    axios.defaults.headers.common['Authorization'] = 'Bearer ' + document.querySelector('meta[name="api-token"]').getAttribute('content');
    //    return axios.post(baseUrl + `/admin/Students/${StudentId}/delete`);
    //},
    //AddStudent(Student) {
    //    axios.defaults.headers.common['Authorization'] = 'Bearer ' + document.querySelector('meta[name="api-token"]').getAttribute('content');
    //    return axios.post(baseUrl + `/admin/Students/Add`, Student);
    //},
    //EditStudent(Student) {
    //    axios.defaults.headers.common['Authorization'] = 'Bearer ' + document.querySelector('meta[name="api-token"]').getAttribute('content');
    //    return axios.post(baseUrl + `/admin/Students/Edit`, Student);
    //},
    ////******************************************* Company Service *********************************
    //GetCompanies() {
    //    axios.defaults.headers.common['Authorization'] = 'Bearer ' + document.querySelector('meta[name="api-token"]').getAttribute('content');
    //    return axios.get(baseUrl + `/Admin/Companies/Get`);
    //},
    //GetCompanies_v1(pageNo, pageSize) {
    //    axios.defaults.headers.common['Authorization'] = 'Bearer ' + document.querySelector('meta[name="api-token"]').getAttribute('content');
    //    return axios.get(baseUrl + `/Admin/Companies/GetCompanies?pageno=${pageNo}&pagesize=${pageSize}`);
    //},
    //DeleteCompany(CompanyId) {
    //    axios.defaults.headers.common['Authorization'] = 'Bearer ' + document.querySelector('meta[name="api-token"]').getAttribute('content');
    //    return axios.post(baseUrl + `/admin/Companies/${CompanyId}/delete`);
    //},
    //AddCompany(Company) {
    //    axios.defaults.headers.common['Authorization'] = 'Bearer ' + document.querySelector('meta[name="api-token"]').getAttribute('content');
    //    return axios.post(baseUrl + `/admin/Companies/Add`, Company);
    //},
    //EditCompany(Company) {
    //    axios.defaults.headers.common['Authorization'] = 'Bearer ' + document.querySelector('meta[name="api-token"]').getAttribute('content');
    //    return axios.post(baseUrl + `/admin/Companies/Edit`, Company);
    //},
    ////********************* Super Packages Service *****************************

    //GetSuperPackages() {
    //    axios.defaults.headers.common['Authorization'] = 'Bearer ' + document.querySelector('meta[name="api-token"]').getAttribute('content');
    //    return axios.get(baseUrl + `/Admin/SuperPackages/Get`);
    //},
    //GetSuperPackages_v1(pageNo, pageSize) {
    //    axios.defaults.headers.common['Authorization'] = 'Bearer ' + document.querySelector('meta[name="api-token"]').getAttribute('content');
    //    return axios.get(baseUrl + `/Admin/SuperPackages/GetSuperPackages?pageno=${pageNo}&pagesize=${pageSize}`);
    //},
    //GetSuperPackagesContainsPackagesOnly(pageNo, pageSize) {
    //    axios.defaults.headers.common['Authorization'] = 'Bearer ' + document.querySelector('meta[name="api-token"]').getAttribute('content');
    //    return axios.get(baseUrl + `/Admin/SuperPackages/GetSuperPackagesContainsPackagesOnly?pageno=${pageNo}&pagesize=${pageSize}`);
    //},
    //DeleteSuperPackage(SuperPackageId) {
    //    axios.defaults.headers.common['Authorization'] = 'Bearer ' + document.querySelector('meta[name="api-token"]').getAttribute('content');
    //    return axios.post(baseUrl + `/admin/SuperPackages/${SuperPackageId}/delete`);
    //},
    //AddSuperPackage(SuperPackage) {
    //    axios.defaults.headers.common['Authorization'] = 'Bearer ' + document.querySelector('meta[name="api-token"]').getAttribute('content');
    //    return axios.post(baseUrl + `/admin/SuperPackages/Add`, SuperPackage);
    //},
    //EditSuperPackage(SuperPackage) {
    //    axios.defaults.headers.common['Authorization'] = 'Bearer ' + document.querySelector('meta[name="api-token"]').getAttribute('content');
    //    return axios.post(baseUrl + `/admin/SuperPackages/Edit`, SuperPackage);
    //},
    ////************* Courses ************
    //GetCoursesBySuperPackageId(pageNo, pageSize,superPakcageId) {
    //    axios.defaults.headers.common['Authorization'] = 'Bearer ' + document.querySelector('meta[name="api-token"]').getAttribute('content');
    //    return axios.get(baseUrl + `/Admin/Courses/GetCoursesBySuperPackageId?pageno=${pageNo}&pagesize=${pageSize}&SuperPackageId=${superPakcageId}`);
    //},
    //GetCoursesByPackageId(pageNo, pageSize, PakcageId) {
    //    axios.defaults.headers.common['Authorization'] = 'Bearer ' + document.querySelector('meta[name="api-token"]').getAttribute('content');
    //    return axios.get(baseUrl + `/Admin/Courses/GetCoursesByPackageId?pageno=${pageNo}&pagesize=${pageSize}&PackageId=${PakcageId}`);
    //},
    //DeleteCourse(CourseId) {
    //    axios.defaults.headers.common['Authorization'] = 'Bearer ' + document.querySelector('meta[name="api-token"]').getAttribute('content');
    //    return axios.post(baseUrl + `/admin/Courses/${CourseId}/delete`);
    //},
    //AddCourse(Course) {
    //    axios.defaults.headers.common['Authorization'] = 'Bearer ' + document.querySelector('meta[name="api-token"]').getAttribute('content');
    //    return axios.post(baseUrl + `/admin/Courses/Add`, Course);
    //},
    //EditCourse(Course) {
    //    axios.defaults.headers.common['Authorization'] = 'Bearer ' + document.querySelector('meta[name="api-token"]').getAttribute('content');
    //    return axios.post(baseUrl + `/admin/Courses/Edit`, Course);
    //},
    ////****************** Packages *************************
    //GetPackagesBySuperPackageId(pageNo, pageSize, superPakcageId) {
    //    axios.defaults.headers.common['Authorization'] = 'Bearer ' + document.querySelector('meta[name="api-token"]').getAttribute('content');
    //    return axios.get(baseUrl + `/Admin/Packages/GetPackagesBySuperPackageId?pageno=${pageNo}&pagesize=${pageSize}&SuperPackageId=${superPakcageId}`);
    //},
    //DeletePackage(PackageId) {
    //    axios.defaults.headers.common['Authorization'] = 'Bearer ' + document.querySelector('meta[name="api-token"]').getAttribute('content');
    //    return axios.post(baseUrl + `/admin/Packages/${PackageId}/delete`);
    //},
    //AddPackage(Package) {
    //    axios.defaults.headers.common['Authorization'] = 'Bearer ' + document.querySelector('meta[name="api-token"]').getAttribute('content');
    //    return axios.post(baseUrl + `/admin/Packages/Add`, Package);
    //},
    //EditPackage(Package) {

    //    axios.defaults.headers.common['Authorization'] = 'Bearer ' + document.querySelector('meta[name="api-token"]').getAttribute('content');
    //    return axios.post(baseUrl + `/admin/Packages/Edit`, Package);
    //},
    //GetPackages(pageNo, pageSize, superPakcageId) {
    //    axios.defaults.headers.common['Authorization'] = 'Bearer ' + document.querySelector('meta[name="api-token"]').getAttribute('content');
    //    return axios.get(baseUrl + `/Admin/Packages/GetPackages?pageno=${pageNo}&pagesize=${pageSize}&SuperPackageId=${superPakcageId}`);
    //},
    // ************************ Branches ******************************



}