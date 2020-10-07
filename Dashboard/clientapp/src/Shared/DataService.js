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
    GetConstituencyPagination(pageNo, pageSize) {
        return axios.get(`/Api/Admin/Constituencies/ConstituencyPagination?pageNo=${pageNo}&pageSize=${pageSize}`);
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
    GetConstituencyDetailsPagination(pageNo, pageSize) {
        return axios.get(`/api/Admin/ConstituencyDetails/ConstituencyDetailsPagination?pageNo=${pageNo}&pageSize=${pageSize}`);
    },
    UpdateConstituencyDetail(constituencyDetail) {
        return axios.put(`/api/Admin/ConstituencyDetails/UpdateConstituencyDetails`, constituencyDetail);
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



}