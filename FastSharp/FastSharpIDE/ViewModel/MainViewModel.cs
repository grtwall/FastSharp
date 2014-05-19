// Copyright 2014 Toni Petrina
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using GalaSoft.MvvmLight.Command;
using Roslyn.Scripting;
using Roslyn.Scripting.CSharp;
using System;

namespace FastSharpIDE.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        #region Roslyn interaction
        private ScriptEngine _engine;
        private Session _session;
        #endregion

        #region Backing fields for the bindable properties
        private string _text;
        private ExecutionResultViewModel _executionResult;
        #endregion

        #region Bindable properties
        public string Text
        {
            get { return _text; }
            set { Set(ref _text, value); }
        }

        public ExecutionResultViewModel ExecutionResult
        {
            get { return _executionResult; }
            set { Set(ref _executionResult, value); }
        }
        #endregion

        public RelayCommand<string> ExecuteCommand { get; set; }

        public MainViewModel()
        {
            ExecuteCommand = new RelayCommand<string>(Execute);
        }

        private void Execute(string code)
        {
            try
            {
                var o = _session.Execute(code);
                var message = o == null ? "** no results from the execution **" : o.ToString();

                ExecutionResult = new ExecutionResultViewModel
                {
                    Message = message,
                    Type = ExecutionResultType.Success
                };
            }
            catch (Exception e)
            {
                ExecutionResult = new ExecutionResultViewModel
                {
                    Message = e.ToString(),
                    Type = ExecutionResultType.Success
                };
            }
        }

        public void Load()
        {
            _engine = new ScriptEngine();
            _session = _engine.CreateSession();
        }
    }
}