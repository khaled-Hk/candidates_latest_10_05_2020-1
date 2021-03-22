import AddStations from './AddStations/AddStations.vue'
import UpdateStation from './UpdateStations/UpdateStations.vue'
import moment from 'moment';

export default {
    name: 'Stations',
    created() {
        //this.GetCenters(this.pageNo);
        this.GetAllRegions();
    },
    components: {
        'add-Stations': AddStations,
        'update-Station': UpdateStation
        //'update-Center': UpdateCenter
    },
    data() {
        return {
            pageNo: 1,
            pageSize: 10,
            pages: 0,
            regions: [],
            constituencies: [],
            constituencyDetails: [],
            centers: [],
            stations:[],
            regionId:null,
            constituencyId: null,
            constituencyDetailId: null,
            centerId: null,
            stationId:null,
            state: 0,
            loading: false

        };
    },
    filters: {
        moment: function (date) {
            if (date === null) {
                return "فارغ";
            }
            // return moment(date).format('MMMM Do YYYY, h:mm:ss a');
            return moment(date).format('MMMM Do YYYY');
        }
    },
    methods: {
        AddStationComponent() {

            if (this.regionId == null) {
                this.$message({
                    type: 'warning',
                    message: 'الرجاء إختيار المنطقة'
                });
            } else if (this.constituencyId == null) {
                this.$message({
                    type: 'warning',
                    message: 'الرجاء إختيار الدائرة الرئيسية'
                });
            } else if (this.constituencyDetailId == null) {
                this.$message({
                    type: 'warning',
                    message: 'الرجاء إختيار الدائرة الفرعية'
                });
            } else if (this.centerId == null) {
                this.$message({
                    type: 'warning',
                    message: 'الرجاء إختيار المركز'
                });
            } else {
                this.state = 1
            }
          
        },
        UpdateStationComponent(stationId) {
            this.state = 2
            this.stationId = stationId;

        },
        GetAllRegions() {
            this.$blockUI.Start();
            this.regionId = null;
            this.constituencyId = null;
            this.constituencyDetailId = null;
            this.centerId = null;
            this.centers = []
            this.$http.GetAllRegions()
                .then(response => {
                   
                    this.regions = response.data;
                    this.$blockUI.Stop();
                })
                .catch((err) => {
                  //  this.loading = false;
                    //this.$blockUI.Stop();
                    this.$blockUI.Stop();
                    return err;
                });
        },

        GetConstituencies() {
            this.$blockUI.Start();
            this.constituencyId = null;
            this.constituencyDetailId = null;
            this.centerId = null;
            this.centers = []
            this.$http.GetAConstituencyBasedOn(this.regionId)
                .then(response => {
                    this.$blockUI.Stop();
                    this.constituencies = response.data;
                    

                })
                .catch((err) => {
                    //  this.loading = false;
                    //this.$blockUI.Stop();
                    this.$blockUI.Stop();
                    return err;
                });
        },
        GetAllConstituencyDetails() {
            this.constituencyDetailId = null;
            this.centerId = null;
            this.centers = []

            this.$blockUI.Start();
            this.$http.GetAllConstituencyDetailsBasedOn(this.constituencyId)
                .then(response => {
                    this.$blockUI.Stop();
                    this.constituencyDetails = response.data;


                })
                .catch((err) => {
                    //  this.loading = false;
                    //this.$blockUI.Stop();
                    this.$blockUI.Stop();
                    return err;
                });
        },
        
        GetCenters() {
            this.$blockUI.Start();
            this.centerId = null;
            
            this.$http.GetCentersBasedOn(this.constituencyDetailId)
                .then(response => {
                    this.$blockUI.Stop();
                    this.centers = response.data;


                })
                .catch((err) => {
                    //  this.loading = false;
                    //this.$blockUI.Stop();
                    this.$blockUI.Stop();
                    return err;
                });
        },
        GetStations(pageNo) {
                this.pageNo = pageNo;
                if (this.pageNo === undefined) {
                    this.pageNo = 1;
                }
                this.loading = true;
                    this.$http.GetStations(this.centerId,this.pageNo, this.pageSize)
                    .then(response => {
                        this.loading = false;
                        this.stations = response.data.stations;
                        this.pages = response.data.count;
                    })
                    .catch((err) => {
                        this.loading = false;
                        //this.$blockUI.Stop();
                        this.pages = 0;
                        return err;
                    });
           
        },
        
        Delete(stationId) {

            this.$confirm('هل حقا تريد مسح المحطة . متـابعة ؟', 'تـحذيـر', {
                confirmButtonText: 'نـعم',
                cancelButtonText: 'إلغاء',
                type: 'warning',
                center: true
            }).then(() => {
                this.$blockUI.Start();
                this.$http.DeleteStation(stationId)
                    .then(response => {
                        this.GetStations(this.pageNo)
                        this.$blockUI.Stop();
                        this.$notify({
                            title: 'تم المسـح بنجاح',
                            dangerouslyUseHTMLString: true,
                            message: '<strong>' + response.data + '</strong>',
                            type: 'success'
                        });

                        this.GetCenters(this.pageNo);
                    })
                    .catch((err) => {
                        this.$blockUI.Stop();
                        this.$message({
                            type: 'error',
                            message: err.response.data
                        });
                    });
            })
        }
    }

}