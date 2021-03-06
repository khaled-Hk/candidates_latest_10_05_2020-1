import AddConstituencyDetails from './AddConstituencyDetails/AddConstituencyDetails.vue'
import UpdateConstituencyDetails from './UpdateConstituencyDetails/UpdateConstituencyDetails.vue'
import moment from 'moment';

export default {
    name: 'ConstituencyDetails',
    created() {
        this.GetConstituencyDetails(this.pageNo);
    },
    components: {
        'add-constituencyDetails': AddConstituencyDetails,
        'update-constituencyDetails': UpdateConstituencyDetails
    },
    data() {
        return {
            pageNo: 1,
            pageSize: 10,
            pages: 0,
            constituencyDetails: [],
            state: 0,
            loading: false,
            constituencyDetailId:null

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
        AddConstituencyDetailsComponent() {
            this.state = 1
        },

        UpdateConstituencyDetailsComponent(constituencyDetailId) {
            this.state = 2
            
            this.constituencyDetailId = constituencyDetailId;
        },
        GetConstituencyDetails(pageNo) {
            this.pageNo = pageNo;
            if (this.pageNo === undefined) {
                this.pageNo = 1;
            }
            this.loading = true;
            this.$http.GetConstituencyDetailsPagination(this.pageNo, this.pageSize)
                .then(response => {
                    this.loading = false;
                    this.constituencyDetails = response.data.constituencyDetails;
                    this.pages = response.data.count;
                })
                .catch((err) => {
                    this.loading = false;
                    //this.$blockUI.Stop();
                    this.pages = 0;
                    return err;
                });
        },
        Delete(constituencyDetailsId) {

            this.$confirm('هل حقا تريد مسح المنطقة . متـابعة ؟', 'تـحذيـر', {
                confirmButtonText: 'نـعم',
                cancelButtonText: 'إلغاء',
                type: 'warning',
                center: true
            }).then(() => {
                this.$blockUI.Start();
                this.$http.DeleteConstituencyDetail(constituencyDetailsId)
                    .then(response => {
                        this.$blockUI.Stop();
                        this.$notify({
                            title: 'تم المسـح بنجاح',
                            dangerouslyUseHTMLString: true,
                            message: '<strong>' + response.data.message + '</strong>',
                            type: 'success'
                        });

                        this.GetConstituencyDetails(this.pageNo);
                    })
                    .catch((err) => {
                        this.$blockUI.Stop();
                        this.$message({
                            type: 'error',
                            message: err.response.data.message
                        });
                    });
            })
        }
    }

}