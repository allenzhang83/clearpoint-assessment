import axios from './axios';
import { TodoItem } from '../models/TodoItem';

export const getTodoItems = async () => {
    const response = await axios.get(`/TodoItems`);
    return response;
}

export const addTodoItem = async (todoItem) => {
    var axiosConfig = {
        headers: {
            'Content-Type': 'application/json;charset=UTF-8',
            "Access-Control-Allow-Origin": "*"
        }
    };

    const response = await axios.post('/TodoItems', todoItem, axiosConfig);
    return response;
}

export const updateTodoItem = async (todoItem) => {
    var axiosConfig = {
        headers: {
            'Content-Type': 'application/json;charset=UTF-8',
            "Access-Control-Allow-Origin": "*"
        }
    };

    const response = await axios.put('/TodoItems', todoItem, axiosConfig);
    return response;
}