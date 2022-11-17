import axios from 'axios';

const axiosInstance = axios.create({
    // read from config in real app
    baseURL: "https://localhost:44397/api"
});

export default axiosInstance;