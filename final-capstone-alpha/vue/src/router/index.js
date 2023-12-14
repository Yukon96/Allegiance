import { createRouter as createRouter, createWebHistory } from 'vue-router'
import { useStore } from 'vuex'

// Import components
import EditHomeView from '../views/home/EditHomeView.vue'
import UserView from '../views/user/UserView.vue';
import HomeView from '../views/home/HomeView.vue';
import LoginView from '../views/LoginView.vue';
import LogoutView from '../views/LogoutView.vue';
import RegisterView from '../views/RegisterView.vue';
import AdminView from '../views/admin/AdminView.vue'
import OTPView from '../views/admin/OTPView.vue'
import ChangePasswordView from '../views/user/ChangePasswordView.vue'
import UserProteinsView from '../views/user/UserProteinsView.vue'

import ProteinView from '../views/protein/ProteinView.vue'
import ProteinListView from '../views/protein/ProteinListView.vue'
import ProteinImportView from '../views/protein/ProteinImportView.vue'
import ProteinDetailView from '../views/protein/ProteinDetailView.vue'

/**
 * The Vue Router is used to "direct" the browser to render a specific view component
 * inside of App.vue depending on the URL.
*
* It also is used to detect whether or not a route requires the user to have first authenticated.
* If the user has not yet authenticated (and needs to) they are redirected to /login
* If they have (or don't need to) they're allowed to go about their way.
*/
const routes = [
  {
    path: '/',
    name: 'home',
    component: HomeView,
    meta: {
      requiresAuth: true
    }
  },
  {
    path: "/admin/editHome",
    name: "editHome",
    component: EditHomeView,
    meta:{
      requiresAuth: true
    }
  },
  {
    path: "/login",
    name: "login",
    component: LoginView,
    meta: {
      requiresAuth: false
    }
  },
  {
    path: "/logout",
    name: "logout",
    component: LogoutView,
    meta: {
      requiresAuth: false
    }
  },
  {
    path: "/register",
    name: "register",
    component: RegisterView,
    meta: {
      requiresAuth: false
    }
  },

  {
    path: "/admin",
    name: "admin",
    component: AdminView,
    meta: {
      requiresAuth: true
    }
  },
  {
    path: '/admin/otp',
    name: 'otp',
    component: OTPView,
    meta: {
      requiresAuth: true
    }
  },

  {
    path: '/user',
    name: 'user',
    component: UserView,
    meta: {
      requiresAuth: true
    }
  },
  {
    path: '/user',
    name: 'user',
    component: UserView,
    meta: {
      requiresAuth: true
    }
  },
  {
    path: '/user/changepassword',
    name: 'change_password',
    component: ChangePasswordView,
    meta: {
      requiresAuth: true
    }
  },
  {
    path: '/user/viewproteins',
    name: 'view_proteins',
    component: UserProteinsView,
    meta: {
      requiresAuth: true
    }
  },

  {
    path: '/protein',
    name: 'protein',
    component: ProteinView,
    meta: {
      requiresAuth: true
    }
  },
  {
    path: '/protein/list',
    name: 'protein_list',
    component: ProteinListView,
    meta: {
      requiresAuth: true
    }
  },
  {
    path: '/protein/:id',
    name: 'protein_detail',
    component: ProteinDetailView,
    props: true,
    meta: {
      requiresAuth: true
    }
  },
  {
    path: '/protein/import',
    name: 'protein_import',
    component: ProteinImportView,
    meta: {
      requiresAuth: true
    }
  },
];

// Create the router
const router = createRouter({
  history: createWebHistory(),
  routes: routes
});

router.beforeEach((to) => {

  // Get the Vuex store
  const store = useStore();

  // Determine if the route requires Authentication
  const requiresAuth = to.matched.some(x => x.meta.requiresAuth);

  // If it does and they are not logged in, send the user to "/login"
  if (requiresAuth && store.state.token === '') {
    return {name: "login"};
  }
  // Otherwise, do nothing and they'll go to their next destination
});

export default router;
