export default {
    name: 'appHeader',    
    created() { 
        window.scrollTo(0, 0);
    },
    data() {
        return {            
            loginDetails: null,
            active: 1,
            menuFlag: [10],
        };
    },
  
    methods: {
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
