namespace sales.Services
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using System.Net.Http.Headers;
    using Common.Models;
    using Newtonsoft.Json;
    using Plugin.Connectivity;
    using sales.Helpers;

    public class ApiService
    {
        // Este metodo verifico que la aplicación tenga conexión
        public async Task<Response> CheckConnection(bool internet)
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                await Task.Delay(50);       //Habilito un delay para que el objeto MainPage no sea null
                return new Response
                {
                    IsSuccess = false,
                    Message = Languages.TurnOnInternet,
                };
            }

            if (internet)    //deseo verificar que la app tiene conexión a internet
            {
                var isReachable = await CrossConnectivity.Current.IsRemoteReachable("google.com");
                if (!isReachable)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = Languages.NoInternet,
                    };
                }
            }
            else
            {
                await Task.Delay(50);       //Habilito un delay para que el poder tener acceso al diccionario de recursos
            }

            return new Response
            {
                IsSuccess = true,
            };
        }

        // En este método obtengo el token de la aplicación pasandole el usuario y la contraseña, este token es válido por un tiempo determinado
        public async Task<TokenResponse> GetToken(string urlBase, string username, string password)
        {
            try
            {
                var client = new HttpClient();
                //client.BaseAddress = new Uri(urlBase);
                var url = $"{urlBase}/Token";
                var response = await client.PostAsync(url,new StringContent(string.Format("grant_type=password&username={0}&password={1}", username, password),
                    Encoding.UTF8, "application/x-www-form-urlencoded"));
                var resultJSON = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<TokenResponse>(resultJSON);
                return result;
            }
            catch
            {
                return null;
            }
        }

        // Este metodo genérico me sirve consumir cualquier servicio api que devuelva una lista
        public async Task<Response> GetList<T>(string urlBase, string prefix, string controller)    
        {
            try
            {
                var client = new HttpClient();
                //client.BaseAddress = new Uri(urlBase);
                var url = $"{urlBase}{prefix}{controller}";
                var response = await client.GetAsync(url);
                var answer = await response.Content.ReadAsStringAsync();

                // verifico si la comunicación falla
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = answer,
                    };
                }

                var list = JsonConvert.DeserializeObject<List<T>>(answer);
                return new Response
                {
                    IsSuccess = true,
                    Result = list,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };                
            }
        }

        public async Task<Response> GetList<T>(string urlBase, string prefix, string controller, string tokenType, string accessToken)
        {
            try
            {
                var client = new HttpClient();
                //client.BaseAddress = new Uri(urlBase);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);
                var url = $"{urlBase}{prefix}{controller}";
                var response = await client.GetAsync(url);
                var answer = await response.Content.ReadAsStringAsync();

                // verifico si la comunicación falla
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = answer,
                    };
                }

                var list = JsonConvert.DeserializeObject<List<T>>(answer);
                return new Response
                {
                    IsSuccess = true,
                    Result = list,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        public async Task<Response> GetList<T>(string urlBase, string prefix, string controller, int id, string tokenType, string accessToken)
        {
            try
            {
                var client = new HttpClient();
                //client.BaseAddress = new Uri(urlBase);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);
                var url = $"{urlBase}{prefix}{controller}/{id}";
                var response = await client.GetAsync(url);
                var answer = await response.Content.ReadAsStringAsync();

                // verifico si la comunicación falla
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = answer,
                    };
                }

                var list = JsonConvert.DeserializeObject<List<T>>(answer);
                return new Response
                {
                    IsSuccess = true,
                    Result = list,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        public async Task<Response> Post<T>(string urlBase, string prefix, string controller, T model)
        {
            try
            {
                var request = JsonConvert.SerializeObject(model);
                var content = new StringContent(request, Encoding.UTF8, "application/json");
                var client = new HttpClient();
                //client.BaseAddress = new Uri(urlBase);
                var url = $"{urlBase}{prefix}{controller}";
                var response = await client.PostAsync(url, content);
                var answer = await response.Content.ReadAsStringAsync();

                // verifico si la comunicación falla
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = answer,
                    };
                }

                var obj = JsonConvert.DeserializeObject<T>(answer);
                return new Response
                {
                    IsSuccess = true,
                    Result = obj,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        public async Task<Response> Post<T>(string urlBase, string prefix, string controller, T model, string tokenType, string accessToken)
        {
            try
            {
                var request = JsonConvert.SerializeObject(model);
                var content = new StringContent(request, Encoding.UTF8, "application/json");
                var client = new HttpClient();
                //client.BaseAddress = new Uri(urlBase);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);
                var url = $"{urlBase}{prefix}{controller}";
                var response = await client.PostAsync(url, content);
                var answer = await response.Content.ReadAsStringAsync();

                // verifico si la comunicación falla
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = answer,
                    };
                }

                var obj = JsonConvert.DeserializeObject<T>(answer);
                return new Response
                {
                    IsSuccess = true,
                    Result = obj,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        public async Task<Response> Delete(string urlBase, string prefix, string controller, int id)
        {
            try
            {
                var client = new HttpClient();
                //client.BaseAddress = new Uri(urlBase);
                var url = $"{urlBase}{prefix}{controller}/{id}";
                var response = await client.DeleteAsync(url);
                var answer = await response.Content.ReadAsStringAsync();

                // verifico si la comunicación falla
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = answer,
                    };
                }

                return new Response
                {
                    IsSuccess = true,                    
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        public async Task<Response> Delete(string urlBase, string prefix, string controller, int id, string tokenType, string accessToken)
        {
            try
            {
                var client = new HttpClient();
                //client.BaseAddress = new Uri(urlBase);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);
                var url = $"{urlBase}{prefix}{controller}/{id}";
                var response = await client.DeleteAsync(url);
                var answer = await response.Content.ReadAsStringAsync();

                // verifico si la comunicación falla
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = answer,
                    };
                }

                return new Response
                {
                    IsSuccess = true,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        public async Task<Response> Put<T>(string urlBase, string prefix, string controller, T model, int id)
        {
            try
            {
                var request = JsonConvert.SerializeObject(model);
                var content = new StringContent(request, Encoding.UTF8, "application/json");
                var client = new HttpClient();
                //client.BaseAddress = new Uri(urlBase);
                var url = $"{urlBase}{prefix}{controller}/{id}";
                var response = await client.PutAsync(url, content);
                var answer = await response.Content.ReadAsStringAsync();

                // verifico si la comunicación falla
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = answer,
                    };
                }

                var obj = JsonConvert.DeserializeObject<T>(answer);
                return new Response
                {
                    IsSuccess = true,
                    Result = obj,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        public async Task<Response> Put<T>(string urlBase, string prefix, string controller, T model, int id, string tokenType, string accessToken)
        {
            try
            {
                var request = JsonConvert.SerializeObject(model);
                var content = new StringContent(request, Encoding.UTF8, "application/json");
                var client = new HttpClient();
                //client.BaseAddress = new Uri(urlBase);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);
                var url = $"{urlBase}{prefix}{controller}/{id}";
                var response = await client.PutAsync(url, content);
                var answer = await response.Content.ReadAsStringAsync();

                // verifico si la comunicación falla
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = answer,
                    };
                }

                var obj = JsonConvert.DeserializeObject<T>(answer);
                return new Response
                {
                    IsSuccess = true,
                    Result = obj,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        public async Task<Response> GetUser(string urlBase, string prefix, string controller, string email, string tokenType, string accessToken)
        {
            try
            {
                var getUserRequest = new GetUserRequest
                {
                    Email = email,
                };

                var request = JsonConvert.SerializeObject(getUserRequest);
                var content = new StringContent(request, Encoding.UTF8, "application/json");

                var client = new HttpClient();
                //client.BaseAddress = new Uri(urlBase);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);
                var url = $"{urlBase}{prefix}{controller}";
                var response = await client.PostAsync(url, content);
                var answer = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = answer,
                    };
                }

                var user = JsonConvert.DeserializeObject<MyUserASP>(answer);
                return new Response
                {
                    IsSuccess = true,
                    Result = user,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        public async Task<FacebookResponse> GetFacebook(string accessToken)
        {
            var requestUrl = new Uri("https://graph.facebook.com/v2.8/me/?fields=name," +
                "picture.width(999),cover,age_range,devices,email,gender," +
                "is_verified,birthday,languages,work,website,religion," +
                "location,locale,link,first_name,last_name," +
                "hometown");
            
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            
            try
            {
                var userJson = await httpClient.GetStringAsync(requestUrl);
                var facebookResponse = JsonConvert.DeserializeObject<FacebookResponse>(userJson);
                return facebookResponse;
            }
            catch (Exception Ex)
            {
                return null;
            }
            
        }

        public async Task<InstagramResponse> GetInstagram(string accessToken)
        {
            var client = new HttpClient();
            var userJson = await client.GetStringAsync(accessToken);
            var InstagramJson = JsonConvert.DeserializeObject<InstagramResponse>(userJson);
            return InstagramJson;
        }

        public async Task<TokenResponse> LoginTwitter(string urlBase, string servicePrefix, string controller, TwitterResponse profile)
        {
            try
            {
                var request = JsonConvert.SerializeObject(profile);
                var content = new StringContent(request, Encoding.UTF8, "application/json");
                var client = new HttpClient();
                //client.BaseAddress = new Uri(urlBase);
                var url = $"{urlBase}{servicePrefix}{controller}";
                var response = await client.PostAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                var tokenResponse = await GetToken(urlBase, profile.IdStr, profile.IdStr);
                return tokenResponse;
            }
            catch
            {
                return null;
            }
        }

        public async Task<TokenResponse> LoginInstagram(string urlBase, string servicePrefix, string controller, InstagramResponse profile)
        {
            try
            {
                var request = JsonConvert.SerializeObject(profile);
                var content = new StringContent(request, Encoding.UTF8, "application/json");
                var client = new HttpClient();
                //client.BaseAddress = new Uri(urlBase);
                var url = $"{urlBase}{servicePrefix}{controller}";
                var response = await client.PostAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                var tokenResponse = await GetToken(urlBase, profile.UserData.Id, profile.UserData.Id);
                return tokenResponse;
            }
            catch
            {
                return null;
            }
        }

        public async Task<TokenResponse> LoginFacebook(string urlBase, string servicePrefix, string controller, FacebookResponse profile)
        {
            try
            {
                var request = JsonConvert.SerializeObject(profile);
                var content = new StringContent(request, Encoding.UTF8, "application/json");
                var client = new HttpClient();
                //client.BaseAddress = new Uri(urlBase);
                var url = $"{urlBase}{servicePrefix}{controller}";
                var response = await client.PostAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                var tokenResponse = await GetToken(urlBase, profile.Id, profile.Id);
                return tokenResponse;
            }
            catch
            {
                return null;
            }
        }

    }
}
