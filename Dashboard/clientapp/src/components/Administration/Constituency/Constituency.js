import AddConstituency from './AddConstituency/AddConstituency.vue'
import UpdateConstituency from './UpdateConstituency/UpdateConstituency.vue'
import moment from 'moment';

export default {
    name: 'Constituency',
    created() {
        this.GetConstituencies(this.pageNo);  
    },
    components: {
        'add-Constituency': AddConstituency,
        'update-Constituency': UpdateConstituency
    },
    data() {
         return {
            pageNo: 1,
            pageSize: 10,
            pages: 0,
            constituencies: [],
            state: 0,
            loading: false,
            constituencyId: 0

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
        UpdateConstituencyComponent(constituencyId) {
            this.state = 2
            this.constituencyId = constituencyId;
        },

        GetConstituencies(pageNo) {
            this.pageNo = pageNo;
            if (this.pageNo === undefined) {
                this.pageNo = 1;
            }
            this.loading = true;
            this.$http.GetConstituencyPagination(this.pageNo, this.pageSize)
                .then(response => {
                        this.loading = false;
                        this.constituencies = response.data.responseMsg.constituencies;
                        this.pages = response.data.responseMsg.count;
                })
                .catch((err) => {
                    this.loading = false;
                    //this.$blockUI.Stop();
                    this.pages = 0;
                    return err;
                });
        },


        Delete(constituencyId)
        {
            this.$confirm('هل حقا تريد مسح المنطقة . متـابعة ؟', 'تـحذيـر', {
                confirmButtonText: 'نـعم',
                cancelButtonText: 'إلغاء',
                type: 'warning',
                center: true
            }).then(() => {
                this.$blockUI.Start();
                this.$http.DeleteConstituency(constituencyId)
                    .then(response => {
                        this.$blockUI.Stop();
                        this.$notify({
                            title: 'تم المسـح بنجاح',
                            dangerouslyUseHTMLString: true,
                            message: '<strong>' + response.data.message + '</strong>',
                            type: 'success'
                        });

                        this.GetConstituencies(this.pageNo);
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