
// firebase.ts
//import dotenv from 'dotenv'
//dotenv.config()
import { initializeApp } from 'firebase/app';
import { getAuth } from 'firebase/auth';
import * as firebase from "firebase/app";
import "firebase/auth";

//firebase.initializeApp({
//    apiKey: 'AIzaSyD0jjAIgTj-zItmJMEMWECGVe9CvI0B2HY',
//    authDomain: 'localhost',
//    projectId: 'sociallogin-152a5',
//    appId: '3542967533',
//});
//export const auth = firebase.auth();
//const googleProvider = new firebase.auth.GoogleAuthProvider()
//export const signInWithGoogle = () => {
//    auth.signInWithPopup(googleProvider).then((res) => {
//        console.log(res.user)
//    }).catch((error) => {
//        console.log(error.message)
//    })
//}

const firebaseConfig = {
    apiKey: 'AIzaSyD0jjAIgTj-zItmJMEMWECGVe9CvI0B2HY',
    authDomain: 'sociallogin-152a5.firebaseapp.com',
    projectId: 'sociallogin-152a5',
    appId: '3542967533',
};

const app = initializeApp(firebaseConfig);
const auth = getAuth(app);

export { auth };



//const handleSignOut = async () => {
//    try {
//        await firebase.auth().signOut();
//        setUser(null);
//    } catch (error) {
//        console.error('Sign out error:', error);
//    }
//};