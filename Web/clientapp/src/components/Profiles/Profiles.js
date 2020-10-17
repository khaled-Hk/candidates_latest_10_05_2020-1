import AddProfile from './AddProfile/AddProfile.vue';
import moment from 'moment';
export default {
    name: 'Profiles',    
    created() {
        this.GetProfiles(this.pageNo);  
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
            Profiles: [],
            state: 0,
            loading:false
          
        };
    },
    methods: {
        AddProfileComponent() {
            this.state = 1;
        },

        GetProfiles(pageNo) {
            this.pageNo = pageNo;
            if (this.pageNo === undefined) {
                this.pageNo = 1;
            }
            this.loading = true;
            this.$http.GetProfiles(this.pageNo, this.pageSize)
                .then(response => {
                    this.loading = false;
                    this.Profiles = response.data.profile;
                    this.pages = response.data.count;
                })
                .catch((err) => {
                    this.loading = false;
                    //this.$blockUI.Stop();
                    this.pages = 0;
                    return err;
                });
        },

        Activate(ProfileId) {
            this.$confirm('هل حقا تريد تفعيل ملف الانتخابات . متـابعة ؟', 'تـحذيـر', {
                confirmButtonText: 'نـعم',
                cancelButtonText: 'إلغاء',
                type: 'warning',
                center: true
            }).then(() => {   
                this.$blockUI.Start();
                this.$http.ActivateProfile(ProfileId)
                    .then(response => {
                        this.$blockUI.Stop();
                        this.$notify({
                            title: 'تم التفعيل بنجاح',
                            dangerouslyUseHTMLString: true,
                            message: '<strong>' + response.data + '</strong>',
                            type: 'success'
                        });  

                        this.GetProfiles(this.pageNo);
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


        Deactivate(ProfileId) {
            this.$confirm('هل حقا تريد إلغاء تفعيل ملف الانتخابات . متـابعة ؟', 'تـحذيـر', {
                confirmButtonText: 'نـعم',
                cancelButtonText: 'إلغاء',
                type: 'warning',
                center: true
            }).then(() => {
                this.$blockUI.Start();
                this.$http.DeActivateProfile(ProfileId)
                    .then(response => {
                        this.$blockUI.Stop();
                        this.$notify({
                            title: 'تم إلغاء التفعيل بنجاح',
                            dangerouslyUseHTMLString: true,
                            message: '<strong>' + response.data + '</strong>',
                            type: 'success'
                        });

                        this.GetProfiles(this.pageNo);
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

        PlayProfile(ProfileId) {
            this.$confirm('هل حقا تريد تشغيل ضبط الملف الانتخابي . متـابعة ؟', 'تـحذيـر', {
                confirmButtonText: 'نـعم',
                cancelButtonText: 'إلغاء',
                type: 'warning',
                center: true
            }).then(() => {
                this.$blockUI.Start();
                this.$http.PlayProfile(ProfileId)
                    .then(response => {
                        this.$blockUI.Stop();
                        this.$notify({
                            title: 'تم تشغيل الملف بنجاح',
                            dangerouslyUseHTMLString: true,
                            message: '<strong>' + response.data + '</strong>',
                            type: 'success'
                        });
                        this.GetProfiles(this.pageNo);
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

        PauseProfile(ProfileId) {
            this.$confirm('هل حقا تريد إيقاف ضبط الملف الانتخابي . متـابعة ؟', 'تـحذيـر', {
                confirmButtonText: 'نـعم',
                cancelButtonText: 'إلغاء',
                type: 'warning',
                center: true
            }).then(() => {
                this.$blockUI.Start();
                this.$http.PauseProfile(ProfileId)
                    .then(response => {
                        this.$blockUI.Stop();
                        this.$notify({
                            title: 'تم إيقاف الملف بنجاح',
                            dangerouslyUseHTMLString: true,
                            message: '<strong>' + response.data + '</strong>',
                            type: 'success'
                        });
                        this.GetProfiles(this.pageNo);
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
