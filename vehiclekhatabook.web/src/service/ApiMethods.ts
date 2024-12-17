// "use server";
import axios, { AxiosRequestConfig } from "axios";
import { getAdminToken, handleApiError } from "./ApiConfig";

const apiClient = axios.create({
  baseURL: process.env.NEXT_PUBLIC_WEB_API_URL,
  headers: {
    "Content-Type": "application/json",
  },
});

const GET = async (url: string) => {
  try {
    const response = await apiClient.get(url);
    return response.data;
  } catch (error: any) {
    console.log(error.message);
    // handleApiError(error);
    throw error;
  }
};

const POST = async (url: string, body: any) => {
  try {
    const response = await apiClient.post(url, body);
    return response.data;
  } catch (error: any) {
    console.log("Error:", error);

    // handleApiError(error);
    throw error;
  }
};

const PUT = async (url: string, body: any) => {
  try {
    const response = await apiClient.put(url, body)
    return response.data;
  } catch (error: any) {
    handleApiError(error);
    throw error;
  }
};
const PATCH = async (url: string, body: any) => {
  try {
    const response = await apiClient.patch(url, body);  // Use PATCH method
    return response.data;  // Return the response data if successful
  } catch (error: any) {
    handleApiError(error);  // Handle any errors that occur
    throw error;  // Re-throw the error after handling
  }
}

const DELETE = async (url: string) => {
  try {
    const response = await apiClient.delete(url);
    return response.data;
  } catch (error: any) {
    // handleApiError(error);
    throw error;
  }
};

export { GET, POST, PUT, DELETE,PATCH };