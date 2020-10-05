export default {
    name: 'AppHeader',    
    created() { 
        
    },
    data() {
        return {            
            loginDetails: null,
            active: 1,
            Tog: 'navbar-toggle',
            result:''
        };
    },
  
    methods: {
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
