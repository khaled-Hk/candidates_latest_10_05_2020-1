import Vue from 'vue';
import VueI18n from 'vue-i18n'
import VueRouter from 'vue-router';
// ************ import elementui + css *****************
import ElementUI from 'element-ui';
require('./../node_modules/element-ui/lib/theme-chalk/index.css');
//*************************************************
//import Vuetify from 'vuetify'
import locale from 'element-ui/lib/locale/lang/en'
import BlockUIService from './Shared/BlockUIService.js';
import Layout from './components/Layout/Layout.vue';
import Home from './components/Home/Home.vue';
import Profiles from './components/Profiles/Profiles.vue';
import Regions from './components/Regions/Regions.vue';
import Branches from './components/Administration/Branches/Branches.vue';
import Constituency from './components/Administration/Constituency/Constituency.vue';
import ConstituencyDetails from './components/Administration/ConstituencyDetails/ConstituencyDetails.vue';
import Centers from './components/Administration/Centers/Centers.vue'
import Stations from './components/Administration/Stations/Stations.vue';
import DataService from './Shared/DataService';


//////////////User Component////////////////////
import Users from './components/Users/Users.vue';
import AddUsers from './components/Users/AddUsers/AddUsers.vue';
import EditUsersProfile from './components/Users/EditUsersProfile/EditUsersProfile.vue';
import ChangePassword from './components/Users/ChangePassword/ChangePassword.vue';
//////////////////////////t////////////////////
//import messages from './i18n';




//Vue.use(Vuetify)
Vue.use(VueI18n);
Vue.use(VueRouter);
Vue.use(ElementUI, { locale });

Vue.config.productionTip = false;

Vue.prototype.$http = DataService;
Vue.prototype.$blockUI = BlockUIService;


export const eventBus = new Vue();

//const i18n = new VueI18n({
//    locale: 'ar', // set locale
//    messages, // set locale messages
//})

const router = new VueRouter({
    mode: 'history',
    base: __dirname,
    linkActiveClass: 'active',
    routes: [
        { path: '/', component: Home },
        { path: '/Profiles', component: Profiles },
        { path: '/Regions', component: Regions },
        { path : '/Administration/Branches', component: Branches },


        { path: '/Users', component: Users },
        { path: '/AddUsers', component: AddUsers },
        { path: '/EditUsersProfile', component: EditUsersProfile },
        { path: '/ChangePassword', component: ChangePassword },  


        { path: '/Administration/Branches', component: Branches },
        { path: '/Administration/Constituency', component: Constituency },
        { path: '/Administration/Subconstituency', component: ConstituencyDetails },
        { path: '/Administration/Centers', component: Centers },
        { path: '/Administration/Stations', component: Stations }
    ]

});

Vue.filter('toUpperCase', function (value) {
    if (!value) return '';
    return value.toUpperCase();
});

new Vue({
    router,
    render: h => {
        return h(Layout);
    }
}).$mount('#app');
