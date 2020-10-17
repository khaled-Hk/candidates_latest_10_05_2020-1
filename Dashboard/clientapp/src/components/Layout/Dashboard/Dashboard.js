import { Affix } from 'vue-affix';
export default {
    name: 'appHeader',    
    created() { 
        window.scrollTo(0, 0);
        this.CheckLoginStatus();
    },
    components: {
        Affix,
    },

    data() {
        return {            
            loginDetails: null,
            active: 1,
            menuFlag: [10],
        };
    },
  
    methods: {
        CheckLoginStatus() {
            try {
                this.loginDetails = JSON.parse(sessionStorage.getItem('currentUser'));
                //this.loginDetails = this.decrypt(sessionStorage.getItem('currentUser'), sessionStorage.getItem('SECRET_KEY'));
                if (this.loginDetails == null) {
                    window.location.href = '/Security/Login';
                }

            } catch (error) {
                window.location.href = '/Security/Login';
            }
        },
        OpenMenuByToggle() {
            var root = document.getElementsByTagName("body")[0]; // '0' to assign the first (and only `HTML` tag)
            if (root.getAttribute("class") == "rtl sidebar-mini rtl-active") {
                root.setAttribute("class", "rtl rtl-active");
            } else {
                root.setAttribute("class", "rtl sidebar-mini rtl-active");
            }
        },

        href(url) {
            this.$router.push(url);
        }

      
    }    
}
