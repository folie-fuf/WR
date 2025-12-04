import axios from 'axios';

const API_BASE_URL = 'http://localhost:5291/api';

const api = axios.create({
  baseURL: API_BASE_URL,
});

export const valeraAPI = {
  // Получить всех Валер
  getAll: () => api.get('/valera'),
  
  // Получить Валеру по ID
  getById: (id) => api.get(`/valera/${id}`),
  
  // Создать нового Валеру
  create: (valeraData) => api.post('/valera', valeraData),
  
  // Удалить Валеру
  delete: (id) => api.delete(`/valera/${id}`),
  
  // Действия
  work: (id) => api.post(`/valera/${id}/work`),
  nature: (id) => api.post(`/valera/${id}/nature`),
  wine: (id) => api.post(`/valera/${id}/wine`),
  bar: (id) => api.post(`/valera/${id}/bar`),
  marginals: (id) => api.post(`/valera/${id}/marginals`),
  sing: (id) => api.post(`/valera/${id}/sing`),
  sleep: (id) => api.post(`/valera/${id}/sleep`),
};

export default api;