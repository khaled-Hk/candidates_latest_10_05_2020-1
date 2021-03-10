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
                    this.ActiveProfile = response.data.profile;
                })
                .catch((err) => {
                    return err;
                });
        },

        GetRuningProfile() {
            this.$http.GetRuningProfile()
                .then(response => {
                    this.RuningProfile = response.data.profile;
                })
                .catch((err) => {
                    return err;
                });
        },

        href(url) {
            this.$router.push(url);
        },

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
