import moment from 'moment';
export default {
    name: 'AppHeader',    
    created() { 
    /* eslint-disable no-debugger */
    /* eslint-enable no-debugger */
        this.GetActiveProfile();
        this.GetRuningProfile()
        setInterval(() => this.GetActiveProfile(), 10000);    
        setInterval(() => this.GetRuningProfile(), 10000);    
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
            success: { confirmButtonText: 'OK', type: 'success', dangerouslyUseHTMLString: true, center: true },
            error: { confirmButtonText: 'OK', type: 'error', dangerouslyUseHTMLString: true, center: true },
            warning: { confirmButtonText: 'OK', type: 'warning', dangerouslyUseHTMLString: true, center: true },
            loginDetails: null,
            active: 1,
            Tog: 'navbar-toggle',
            result: '',
            ActiveProfile: null,
            ActiveProfileflage: false,
            RuningProfile: null,
            RuningProfileId: null,
            RuningProfileFlage:false,
        };
    },
  
    methods: {
        GetActiveProfile() {
            this.$http.GetActiveProfile()
                .then(response => {
                    if (!this.ActiveProfileflage) {
                        this.ActiveProfileflage = true;
                        this.ActiveProfile = response.data.profile;
                        this.ActiveProfileId = response.data.id
                        return;
                    }

                    if (this.ActiveProfileflage) {
                        if (this.ActiveProfileId != response.data.id && response.data.id != undefined) {
                            this.ActiveProfile = response.data.profile;
                            this.ActiveProfileId = response.data.id
                            this.$alert('<h4>' + 'تم تغير تاريخ تفـعيل الانتخابات' + '</h4>', '', this.warning);
                        } else {
                            this.ActiveProfile = response.data.profile;
                            this.ActiveProfileId = response.data.id
                        }
                    }
                })
                .catch((err) => {
                    return err;
                });
        },

        GetRuningProfile() {
            this.$http.GetRuningProfile()
                .then(response => {
                    
              // /* eslint-disable no-debugger */
                    //   debugger;
                    if (!this.RuningProfileFlage) {
                        this.RuningProfileFlage = true;
                        this.RuningProfile = response.data.profile;
                        this.RuningProfileId = response.data.id
                        return;
                    }

                    if (this.RuningProfileFlage) {
                        if (this.RuningProfileId != response.data.id && response.data.id != undefined) {
                            this.RuningProfile = response.data.profile;
                            this.RuningProfileId = response.data.id
                            this.$alert('<h4>' + 'تم تغير ضبط الانتخابات للنظام الرجاء التأكد عند ادخال البيانات' + '</h4>', '', this.warning);
                        }
                    } else {
                        this.RuningProfile = response.data.profile;
                        this.RuningProfileId = response.data.id
                    }
                })
                .catch((err) => {
                    return err;
                });
        },

        href(url) {
            this.$router.push(url);
        },



        //navbar-toggle toggled
        Logout() {
            window.location.href = "/Security/Login";
        },

        IsTogled() {
            if (this.Tog =='navbar-toggle toggled') {
                this.Tog = 'navbar-toggle';
            } else {
                this.Tog = 'navbar-toggle toggled';
            }
        },
        OpenMenuByToggle() {
            var root = document.getElementsByTagName("html")[0]; // '0' to assign the first (and only `HTML` tag)
            if (root.getAttribute("class") == "perfect-scrollbar-on") {
                root.setAttribute("class", "perfect-scrollbar-on nav-open");
            } else {
                root.setAttribute("class", "perfect-scrollbar-on");
            }
        }
      
      
    }    
}
