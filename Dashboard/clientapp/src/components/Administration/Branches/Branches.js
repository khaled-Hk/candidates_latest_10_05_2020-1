import AddBranch from './AddBranch/AddBranch.vue';
import moment from 'moment';
export default {
    name: 'Branches',    
    created() {
        this.GetBranches(this.pageNo);  
    },
    components: {
        'add-Branch': AddBranch,
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
    data() {
        return {  
            pageNo: 1,
            pageSize: 10,
            pages: 0,  
            Regions: [],
            state: 0,
            loading:false
        };
    },
    methods: {
        AddBranchComponent() {
            this.state = 1;
        },

        GetBranches(pageNo) {
            this.pageNo = pageNo;
            if (this.pageNo === undefined) {
                this.pageNo = 1;
            }
            this.loading = true;
            this.$http.GetBranches(this.pageNo, this.pageSize)
                .then(response => {
                    this.loading = false;
                    this.Branches = response.data.branches;
                    this.pages = response.data.count;
                })
                .catch((err) => {
                    this.loading = false;
                    //this.$blockUI.Stop();
                    this.pages = 0;
                    return err;
                });
        },

        Delete(BranchId) {
            this.$confirm('هل حقا تريد مسح الـفرع . متـابعة ؟', 'تـحذيـر', {
                confirmButtonText: 'نـعم',
                cancelButtonText: 'إلغاء',
                type: 'warning',
                center: true
            }).then(() => {   
                this.$blockUI.Start();
                this.$http.DeleteBranche(BranchId)
                    .then(response => {
                        this.$blockUI.Stop();
                        this.$notify({
                            title: 'تم المسـح بنجاح',
                            dangerouslyUseHTMLString: true,
                            message: '<strong>' + response.data + '</strong>',
                            type: 'success'
                        });  
                        this.GetBranches(this.pageNo);
                    })
                    .catch((err) => {
                        this.$blockUI.Stop();
                        this.$message({
                            type: 'error',
                            message: err.response.data
                        });
                    });
            })
        },
    }    
}
