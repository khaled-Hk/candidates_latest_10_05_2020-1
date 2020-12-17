import moment from 'moment';
export default {
    name: 'AppHeader',    
    created() { 
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
            ActiveProfileOld: null,
            RuningProfile: null,
            RuningProfileOld: null,
        };
    },
  
    methods: {
        GetActiveProfile() {
            this.$http.GetActiveProfile()
                .then(response => {
                /* eslint-disable no-debugger */
                    debugger;
                    if (this.ActiveProfile == 0) {
                        this.ActiveProfile = response.data.profile;
                    } else {
                        if (response.data.profile != null) {
                            if (this.ActiveProfile != null) {
                                if (this.ActiveProfile.name !== response.data.profile.name) {
                                    this.ActiveProfile = response.data.profile;
                                    this.$alert('<h4>' + 'تم تغير تاريخ تفـعيل الانتخابات' + '</h4>', '', this.warning);
                                }
                            } else {
                                this.ActiveProfile = response.data.profile;
                                this.$alert('<h4>' + 'تم تغير تاريخ تفـعيل الانتخابات' + '</h4>', '', this.warning);
                            }
                        }
                    }
                    this.ActiveProfile = response.data.profile;
                })
                .catch((err) => {
                    return err;
                });
        },

        GetRuningProfile() {
            this.$http.GetRuningProfile()
                .then(response => {
                    if (this.RuningProfile == 0) {
                        this.RuningProfile = response.data.profile;
                    } else {
                        if (response.data.profile != null) {
                            if (this.RuningProfile != null) {
                                if (this.RuningProfile.name !== response.data.profile.name) {
                                    this.RuningProfile = response.data.profile;
                                    this.$alert('<h4>' + 'تم تغير تاريخ تفـعيل الانتخابات' + '</h4>', '', this.warning);
                                }
                            } else {
                                this.RuningProfile = response.data.profile;
                                this.$alert('<h4>' + 'تم تغير تاريخ تفـعيل الانتخابات' + '</h4>', '', this.warning);
                            }
                        }
                    }
                    this.RuningProfile = response.data.profile;
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
