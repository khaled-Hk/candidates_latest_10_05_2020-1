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
            menuFlag: [20],
        };
    },
  
    methods: {
        href(url, id) {
            //this.$set(this.menuFlag, 0, '');
            //this.$set(this.menuFlag, 1, '');
            //this.$set(this.menuFlag, 4, '');
            for (var i = 0; i < 12; i++) {
                if (i == id) {
                    this.$set(this.menuFlag, id, 'active');
                } else {
                    this.$set(this.menuFlag, i, '');
                }
            }
            this.$router.push(url);
        },
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

        //href(url) {
        //    this.$router.push(url);
        //}

      
    }    
}
