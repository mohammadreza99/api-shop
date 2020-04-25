using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace ShopApi.DataLayer.DataStructure
{
    public class CrudOperationDs
    {
        #region properties
        private bool _resultFlag = true;
        private double _resultCode = 200;
        private bool _tokenExist = true;
        private List<string> _errorMessages;
        private List<string> _messages;
        public UserDs UserInfo { get; set; }
        public string Token { get; set; }
        public object Result { set; get; }
        public bool TokenExist
        {
            set => _tokenExist = value;
            get => _tokenExist;
        }
        public bool ResultFlag
        {
            set
            {
                _resultFlag = value;
                if (!value)
                {
                    ResultCode = 500;
                    _resultFlag = false;
                    if (ErrorMessages == null)
                        ErrorMessages = new List<string>();
                }
                else
                {
                    _resultFlag = true;
                    ResultCode = 200;
                }
            }
            get
            {
                if (_errorMessages != null && _errorMessages.Any())
                {
                    ResultCode = 500;
                    ResultFlag = false;
                    return false;
                }
                return _resultFlag;
            }
        }
        public double ResultCode
        {
            set
            {
                _resultCode = value;
                if (value != -1 && ErrorMessages == null) 
                    ErrorMessages = new List<string>();
            }
            get => _resultCode;
        }
        public List<string> ErrorMessages
        {
            set => _errorMessages = value.ToList();
            get { return _errorMessages ??= new List<string>(); }
            // IF _errorMessages == null THEN _errorMessages = new List<string>();
        }
        public List<string> Messages
        {
            set => _messages = value;
            get => _messages ??= new List<string>();
        }

        #endregion properties

        #region Methods

        public CrudOperationDs()
        {
            ResultFlag = true;
            ResultCode = 200;
        }

        public CrudOperationDs(CrudOperationDs crud)
        {
            Set(crud);
        }

        private void Set(CrudOperationDs crud)
        {
            ResultFlag = crud.ResultFlag;
            ResultCode = crud.ResultCode;
            Result = crud.Result;
            ErrorMessages = crud.ErrorMessages;
            Messages = crud.Messages;
        }

        public CrudOperationDs SetError(Exception ex)
        {
            ResultCode = 500;
            ResultFlag = false;
            do
            {
                ErrorMessages.Add(ex.Message);
                ex = ex.InnerException;
            } while (ex != null);
            return this;
        }

        public void SetError(string message)
        {
            UserInfo = null;
            ResultFlag = false;
            ResultCode = 500;
            ErrorMessages.Add(message);
        }

        public void SetError(List<string> messages)
        {
            ResultCode = 500;
            ResultFlag = false;
            UserInfo = null;
            ErrorMessages.AddRange(messages);
        }

        public static CrudOperationDs Default()
        {
            return new CrudOperationDs();
        }
        #endregion Methods  
    }
}
