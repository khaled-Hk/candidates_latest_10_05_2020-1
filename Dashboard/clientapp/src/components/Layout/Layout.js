import AppHeader from './AppHeader/AppHeader.vue';
import AppFooter from './AppFooter/AppFooter.vue';
import Dashboard from './Dashboard/Dashboard.vue';

export default {
    name: 'layout',   
    components: {
        'app-header': AppHeader,
        'app-footer': AppFooter,
        'app-dasboard': Dashboard,
       
    },
    created() {
        localStorage.clear();
        this.$blockUI.$loading = this.$loading;
     //   this.IsLogin();
    },
    data() {
        return {
            isAuthenticated: false,
            isActive: false
        };
    },
    methods: {
      //change body sidebar-mini
        //IsLogin() {
        //    this.$http.IsLogin()
        //        .then(response => {
        //            this.isAuthenticated = response.data;
        //            if (!this.isAuthenticated) {
        //                window.location.href = "/Login";
        //            }
        //        })
        //        .catch((err) => {
        //            return err;
        //        });
        //},




    }
    
}
