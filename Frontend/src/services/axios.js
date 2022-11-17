import axios from 'axios';
import { toast } from 'react-toastify';

const axiosInstance = axios.create({
    // read from config in real app
    baseURL: "https://localhost:44397/api"
});

axiosInstance.interceptors.response.use(
    (response) => {return response},
    (error) => errorHandler(error)
);

const errorHandler = (error) => {
    toast.error(`${error.response.data}`, {
    position: "top-center",
    autoClose: false,
    hideProgressBar: false,
    closeOnClick: true,
    pauseOnHover: true,
    draggable: true,
    progress: 0,
    });  
    
    return Promise.reject({ ...error })
}

export default axiosInstance;