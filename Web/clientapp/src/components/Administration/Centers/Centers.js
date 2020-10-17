import AddCenter from './AddCenter/AddCenter.vue'
import UpdateCenter from './UpdateCenter/UpdateCenter.vue'
import moment from 'moment';

export default {
    name: 'Centers',
    created() {
        this.GetCenters(this.pageNo);
    },
    components: {
        'add-Center': AddCenter,
        'update-Center':UpdateCenter
    },
    data() {
        return {
            pageNo: 1,
            pageSize: 10,
            pages: 0,
            centers: [],
            state: 0,
            loading: false,
            centerId: 0

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
        AddConstituencyComponent() {
            this.state = 1
        },
        UpdateCenterComponent(centerId) {
            this.state = 2
            this.centerId = centerId;
           
        },

        GetCenters(pageNo) {
            this.pageNo = pageNo;
            if (this.pageNo === undefined) {
                this.pageNo = 1;
            }
            this.loading = true;
            this.$http.GetCentersPagination(this.pageNo, this.pageSize)
                .then(response => {
                    this.loading = false;
                    this.centers = response.data.centers;
                    this.pages = response.data.count;
                })
                .catch((err) => {
                    this.loading = false;
                    //this.$blockUI.Stop();
                    this.pages = 0;
                    return err;
                });
        },
        Delete(centerId) {
          
            this.$confirm('هل حقا تريد مسح المركز . متـابعة ؟', 'تـحذيـر', {
                confirmButtonText: 'نـعم',
                cancelButtonText: 'إلغاء',
                type: 'warning',
                center: true
            }).then(() => {
                this.$blockUI.Start();
                this.$http.DeleteCenter(centerId)
                    .then(response => {
                        this.$blockUI.Stop();
                        this.$notify({
                            title: 'تم المسـح بنجاح',
                            dangerouslyUseHTMLString: true,
                            message: '<strong>' + response.data.message + '</strong>',
                            type: 'success'
                        });

                        this.GetCenters(this.pageNo);
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