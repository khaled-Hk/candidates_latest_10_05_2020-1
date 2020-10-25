import AddProfile from './AddOffices/AddOffices.vue';
import moment from 'moment';
export default {
    name: 'Profiles',    
    created() {
        this.GetOffice(this.pageNo);  
    },
    components: {
        'add-Profile': AddProfile,
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
            state: 0,
            loading: false,

            Office:[],
          
        };
    },
    methods: {
        Add() {
            this.state = 1;
        },



        GetOffice(pageNo) {
            this.pageNo = pageNo;
            if (this.pageNo === undefined) {
                this.pageNo = 1;
            }
            this.loading = true;
            this.$http.GetOffice(this.pageNo, this.pageSize)
                .then(response => {
                    this.loading = false;
                    this.Office = response.data.office;
                    this.pages = response.data.count;
                })
                .catch((err) => {
                    this.loading = false;
                    //this.$blockUI.Stop();
                    this.pages = 0;
                    return err;
                });
        },

        deleteOffice(id) {
            this.$confirm('هل حقا تريد حدف المكتب الانتخابي . متـابعة ؟', 'تـحذيـر', {
                confirmButtonText: 'نـعم',
                cancelButtonText: 'إلغاء',
                type: 'warning',
                center: true
            }).then(() => {   
                this.$blockUI.Start();
                this.$http.deleteOffice(id)
                    .then(response => {
                        this.$blockUI.Stop();
                        this.$notify({
                            title: 'تم الحدف بنجاح',
                            dangerouslyUseHTMLString: true,
                            message: '<strong>' + response.data + '</strong>',
                            type: 'success'
                        });  

                        this.GetOffice(this.pageNo);
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
